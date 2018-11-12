using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Components.Graphics;

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

            var body = _componentFactory.GetComponent<BodyComponent>();
            var draw = _componentFactory.GetComponent<PriorityTextureComponent>();
            var speed = _componentFactory.GetComponent<SpeedComponent>();

            entity.AddComponent(body);
            entity.AddComponent(draw);
            entity.AddComponent(speed);

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