using System;
using Dunkel.Game.Commands.Game;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Components.Utilities;
using Dunkel.Game.Cosmos;
using Dunkel.Game.Entities;

namespace Dunkel.Game.Commands
{
    public class CommandManager
    {
        private readonly EntityFactory _entityFactory;

        public CommandManager(EntityFactory entityFactory)
        {
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
        }
        
        public void Update(World world, ICommand cmd, long playerId)
        {
            switch (cmd.Type)
            {
                case CommandType.GameSpawn:
                    HandleGameSpawn(world, (Game.SpawnCommand)cmd, playerId);
                    break;
                case CommandType.GameMove:
                    HandleGameMove(world, (Game.MoveCommand)cmd, playerId);
                    break;
            }
        }

        private void HandleGameSpawn(World world, SpawnCommand cmd, long playerId)
        {
            Entity entity;

            switch (cmd.ClassificationType)
            {
                case ClassificationType.Ship:
                    entity = _entityFactory.GetShip();
                    break;
                default:
                    throw new ArgumentException($"Unknown classification type: {cmd.ClassificationType}");
            }

            entity.AddComponent<PlayerIdComponent>(x => x.PlayerId = playerId);
            entity.GetComponent<BodyComponent>().SetPosition(cmd.Position.X, cmd.Position.Y);

            world.AddEntity(entity);
        }

        private void HandleGameMove(World world, MoveCommand cmd, long playerId)
        {
            foreach (var entityId in cmd.EntityIds)
            {
                if (   world.TryGetEntity(entityId, out var entity) 
                    && entity.TryGetComponent<SpeedComponent>(out var component))
                {
                    component.Destination = cmd.Destination;
                }
            }
        }
    }
}