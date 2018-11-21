using System;
using System.Collections.Generic;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Components.Graphics;
using Dunkel.Game.ComponentSystems;
using Dunkel.Game.Entities;
using Dunkel.Game.Input;
using Dunkel.Game.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.Cosmos
{
    public class World
    {
        public long CurrentTick { get; private set; }

        private readonly ComponentSystemManager _componentSystemManager;
        private readonly Selector _selector;
        private readonly Dictionary<int, Entity> _entities;
        private readonly List<Entity> _selectedEntities;

        public World(ComponentSystemManager componentSystemManager, Selector selector)
        {
            _componentSystemManager = componentSystemManager ?? throw new ArgumentNullException(nameof(componentSystemManager));
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));

            _entities = new Dictionary<int, Entity>();
            _selectedEntities = new List<Entity>();
        }

        public void Input(InputManager im)
        {
            InputSelector(im);
        }

        public void Update()
        {
            _componentSystemManager.Update();

            CurrentTick++;
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            _componentSystemManager.Draw(sb, delta);

            _selector.Draw(sb);
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity.Id, entity);
        }

        public Entity GetEntity(int id)
        {
            _entities.TryGetValue(id, out var entity);

            return entity;
        }

        public Entity[] GetEntitiesWithin(Rectangle box)
        {
            var entities = new List<Entity>();
            
            foreach (var entity in _entities)
            {
                if (entity.Value.TryGetComponent<BodyComponent>(out var body) && body.Intersects(box))
                {
                    entities.Add(entity.Value);
                }
            }

            return entities.ToArray();
        }

        private void InputSelector(InputManager im)
        {
            _selector.Input(im);

            if (_selector.HasSelected)
            {
                var entities = GetEntitiesWithin(_selector.Box);

                foreach (var entity in _selectedEntities)
                {
                    entity.RemoveComponent<SelectedEntityComponent>();
                }

                foreach (var entity in entities)
                {
                    entity.AddComponent<SelectedEntityComponent>();
                }

                _selectedEntities.Clear();
                _selectedEntities.AddRange(entities);
            }
        }
    }
}