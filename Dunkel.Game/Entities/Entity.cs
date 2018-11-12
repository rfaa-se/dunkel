using System;
using System.Collections.Generic;
using System.Linq;
using Dunkel.Game.Components;

namespace Dunkel.Game.Entities
{
    public class Entity
    {
        public static event Action<Entity, IComponent> OnComponentAdded;
        public static event Action<Entity, IComponent> OnComponentRemoved;
        public static event Action<Entity> OnDeath;

        public int Id { get; } = _idCounter++;

        private static int _idCounter;

        private readonly EntityFactory _entityFactory;
        private readonly ComponentFactory _componentFactory;
        private readonly Dictionary<Type, IComponent> _components;

        public Entity(EntityFactory entityFactory, ComponentFactory componentFactory)
        {
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _componentFactory = componentFactory ?? throw new ArgumentNullException(nameof(componentFactory));

            _components = new Dictionary<Type, IComponent>();
        }

        public static void ResetIdCounter() => _idCounter = 0;

        public void Die()
        {
            OnDeath?.Invoke(this);

            var components = _components.Values.ToList();

            foreach (var component in components)
            {
                RemoveRecycleComponent(component);
            }

            _entityFactory.Recycle(this);
        }

        public void AddComponent(IComponent component)
        {
            if (_components.TryGetValue(component.GetType(), out var oldComponent))
            {
                RemoveRecycleComponent(oldComponent);
            }

            _components[component.GetType()] = component;
            OnComponentAdded?.Invoke(this, component);
        }

        public void RemoveComponent<T>() where T : class, IComponent
        {
            if (_components.TryGetValue(typeof(T), out var component))
            {
                RemoveRecycleComponent(component);
            }
        }

        public T GetComponent<T>() where T : class, IComponent
        {
            _components.TryGetValue(typeof(T), out var component);
            return component as T;
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        private void RemoveRecycleComponent(IComponent component)
        {
            _components.Remove(component.GetType());
            OnComponentRemoved?.Invoke(this, component);
            _componentFactory.Recycle(component);
        }
    }
}