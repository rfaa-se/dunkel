using System;
using System.Collections.Generic;

namespace Dunkel.Game.Components
{
    public class ComponentFactory
    {
        private readonly Dictionary<Type, Stack<IComponent>> _components;

        public ComponentFactory()
        {
            _components = new Dictionary<Type, Stack<IComponent>>();
        }

        public void Initialize()
        {
            _components.Clear();
        }

        public void Recycle(IComponent component)
        {
            component.Reset();
            _components[component.GetType()].Push(component);
        }

        public T GetComponent<T>() where T : class, IComponent, new()
        {
            if (!_components.TryGetValue(typeof(T), out var pool))
            {
                pool = new Stack<IComponent>();
                _components[typeof(T)] = pool;
            }

            if (!pool.TryPop(out var component))
            {
                PopulatePool<T>();
                component = pool.Pop();
            }

            return component as T;
        }

        private void PopulatePool<T>() where T : class, IComponent, new()
        {
            var pool = _components[typeof(T)];
            
            for (var i = 0; i < 100; i++)
            {
                pool.Push(new T());
            }
        }
    }
}