using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Components.Graphics;
using Dunkel.Game.Utilities;

namespace Dunkel.Game.Entities
{
    public class EntityFactory
    {
        private readonly ComponentFactory _componentFactory;
        private readonly Stack<Entity> _entityPool;

        public EntityFactory(ComponentFactory componentFactory)
        {
            _componentFactory = componentFactory ?? throw new ArgumentNullException(nameof(componentFactory));
            
            _entityPool = new Stack<Entity>();
        }

        public void Initialize()
        {
            Entity.ResetIdCounter();
            _entityPool.Clear();
            PopulatePool();
        }

        public void Recycle(Entity entity)
        {
            _entityPool.Push(entity);
        }

        public Entity GetShip()
        {
            var entity = GetEntity();
            
            entity.AddComponent<BodyComponent>(x => x.SetDimension(50, 50));
            entity.AddComponent<PriorityTextureComponent>();
            entity.AddComponent<SpeedComponent>(x => x.Velocity = new flint(5, 25));
            entity.AddComponent<ClassificationComponent>(x => x.Type = ClassificationType.Ship);

            return entity;
        }

        private Entity GetEntity()
        {
            if (!_entityPool.TryPop(out var entity))
            {
                PopulatePool();
                entity = _entityPool.Pop();
            }

            return entity;
        }

        private void PopulatePool()
        {
            for (var i = 0; i < 100; i++)
            {
                _entityPool.Push(new Entity(this, _componentFactory));
            }
        }
    }
}