using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Entities;

namespace Dunkel.Game.ComponentSystems.Update
{
    public class PreviousBodySystem : IUpdateComponentSystem
    {
        public int Priority => 10;
        
        private readonly Dictionary<int, BodyComponent> _nodes;
        private readonly Type _bodyComponentType;

        public PreviousBodySystem()
        {
            _nodes = new Dictionary<int, BodyComponent>();
            _bodyComponentType = typeof(BodyComponent);
            
            Entity.OnComponentAdded += HandleComponentAdded;
            Entity.OnComponentRemoved += HandleComponentRemoved;
        }

        public void Update()
        {
            foreach (var body in _nodes.Values)
            {
                body.PrevX = body.X;
                body.PrevY = body.Y;
                body.PrevWidth = body.Width;
                body.PrevHeight = body.Height;
                body.PrevRotation = body.Rotation;
            }
        }

        private void HandleComponentAdded(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);

            if (entity.TryGetComponent<BodyComponent>(out var body))
            {
                _nodes[entity.Id] = body;
            }
        }

        private void HandleComponentRemoved(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);
        }

        private bool IsRelevantComponent(IComponent component)
        {
            var type = component.GetType();

            return type == _bodyComponentType;
        }
    }
}