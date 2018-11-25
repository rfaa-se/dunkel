using System;
using System.Collections.Generic;
using Dunkel.Game.Commands;
using Dunkel.Game.Input;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Commands.Game;
using Dunkel.Game.Network;
using Dunkel.Game.Network.Packets.Game;
using Dunkel.Game.Options;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq;
using Dunkel.Game.Cosmos;

namespace Dunkel.Game.States
{
    public class GameState
    {
        public bool IsStalling { get; private set; }

        private readonly CommandManager _commandManager;
        private readonly NetworkManager _networkManager;
        private readonly World _world;
        private readonly List<ICommand> _queuedCommands;
        private readonly Dictionary<long, (long playerId, ICommand[] commands)[]> _updates;
        private readonly Dictionary<long, int> _playerIndexes;
        private readonly EngineOptions _options;
        private readonly ILogger<GameState> _logger;

        public GameState(CommandManager commandManager, NetworkManager networkManager, World world,
            IOptions<EngineOptions> options, ILogger<GameState> logger)
        {
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _queuedCommands = new List<ICommand>();
            _updates = new Dictionary<long, (long, ICommand[])[]>();
            _playerIndexes = new Dictionary<long, int>();

            _playerIndexes[1337] = 0;
            _playerIndexes[1338] = 1;

            _networkManager.Packets.OnGameUpdate += HandleGameUpdate;
        }

        public void Input(InputManager im)
        {
            _world.Input(im, _queuedCommands);
        }

        public void Update()
        {
            IsStalling = 
                      !_updates.TryGetValue(_world.CurrentTick, out var playersCommands)
                   || playersCommands.Any(x => x.commands == null);

            if (IsStalling)
            {
                // we must allow the game to progress for the first X ticks without previous updates
                if (_world.CurrentTick < _options.TicksFutureSchedule)
                {
                    IsStalling = false;
                    playersCommands = new (long, ICommand[])[0];
                }
                else
                {
                    _logger.LogDebug("Stalling at tick {tick}.", _world.CurrentTick);
                    return;
                }
            }

            // run all commands
            foreach (var playerCommands in playersCommands)
            {
                foreach (var command in playerCommands.commands)
                {
                    _commandManager.Update(_world, command, playerCommands.playerId);
                }
            }

            // send current queued commands and then clear the command queue
            _networkManager.SendPacket(new UpdatePacket(_world.CurrentTick, _queuedCommands.ToArray()));
            _queuedCommands.Clear();
            
            // and lastly update the world
            _world.Update();
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            // if we're stalling we need to set the delta to 1f to avoid interpolation glitching
            if (IsStalling) { delta = 1f; }

            _world.Draw(sb, delta);
        }

        private void HandleGameUpdate(UpdatePacket packet)
        {
            if (!_updates.TryGetValue(packet.Tick, out var updates))
            {
                updates = new (long, ICommand[])[2]; // TODO: 2 should be current amount of players
                _updates.Add(packet.Tick, updates);
            }
            
            var playerIndex = _playerIndexes[packet.ClientId];
            updates[playerIndex].playerId = packet.ClientId;
            updates[playerIndex].commands = packet.Commands;
        }
    }
}