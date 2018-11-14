using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Entities;
using Dunkel.Game.Commands;
using Dunkel.Game.ComponentSystems;
using Dunkel.Game.Input;
using Dunkel.Game.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Dunkel.Game.Components.Attributes;
using Microsoft.Xna.Framework;
using Dunkel.Game.Commands.Game;
using Dunkel.Game.Network;
using Dunkel.Game.Network.Packets.Game;

namespace Dunkel.Game.States
{
    public class GameState
    {
        private readonly ComponentSystemManager _componentSystemManager;
        private readonly CommandManager _commandManager;
        private readonly NetworkManager _networkManager;
        private readonly SelectionBox _selectionBox;
        private readonly EntityFactory _entityFactory;
        private readonly List<Entity> _entities;
        private readonly Queue<Point> _queuedEntites;
        private readonly List<ICommand> _queuedCommands;
        private readonly Dictionary<long, ICommand[]> _updates;

        private bool _isStalling;
        private long _tickCurrent;

        public GameState(ComponentSystemManager componentSystemManager, CommandManager commandManager, 
            NetworkManager networkManager, SelectionBox selectionBox, EntityFactory entityFactory)
        {
            _componentSystemManager = componentSystemManager ?? throw new ArgumentNullException(nameof(componentSystemManager));
            _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
            _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));
            _selectionBox = selectionBox ?? throw new ArgumentNullException(nameof(selectionBox));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            
            _entities = new List<Entity>();
            _queuedCommands = new List<ICommand>();
            _queuedEntites = new Queue<Point>();
            _updates = new Dictionary<long, ICommand[]>();

            _networkManager.Packets.OnGameUpdate += HandleGameUpdate;
        }

        public void Input(InputManager im)
        {
            _selectionBox.Input(im);

            if (im.IsMouseRightPressed())
            {
                //_queuedEntites.Enqueue(im.GetMousePosition());
                _queuedCommands.Add(new SpawnCommand(im.GetMousePosition(), ClassificationType.Ship));
            }
        }

        public void Update()
        {
            _isStalling = !_updates.TryGetValue(_tickCurrent, out var commands);

            if (_isStalling)
            {
                System.Diagnostics.Debug.WriteLine($"{_tickCurrent}");
                return;
            }

            foreach (var command in commands)
            {
                switch (command.Type)
                {
                    case CommandType.Spawn:
                    {
                        var c = (SpawnCommand)command;
                        Entity entity = null;

                        switch (c.ClassificationType)
                        {
                            case ClassificationType.Ship:
                                entity = _entityFactory.GetShip();
                                break;
                        }

                        entity.GetComponent<BodyComponent>().SetPosition(c.Point.X, c.Point.Y);
                        _entities.Add(entity);
                        break;
                    }
                }
            }

            // send current queued commands and then clear the command queue
            _networkManager.SendPacket(new UpdatePacket(_tickCurrent, _queuedCommands.ToArray()), _tickCurrent);
            _queuedCommands.Clear();

            _componentSystemManager.Update();

            _tickCurrent++;
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            if (_isStalling) { delta = 1f; }

            _componentSystemManager.Draw(sb, delta);

            _selectionBox.Draw(sb, delta);
        }

        private void HandleGameUpdate(UpdatePacket packet)
        {
            _updates.Add(packet.Tick, packet.Commands);
        }
    }
}