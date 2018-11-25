using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Entities;
using Dunkel.Game.Utilities;

namespace Dunkel.Game.ComponentSystems.Update
{
    public class SpeedSystem : IUpdateComponentSystem
    {
        public int Priority => 19;

        private readonly Dictionary<int, (SpeedComponent speed, BodyComponent body)> _nodes;
        private readonly Type _speedComponentType;
        private readonly Type _bodyComponentType;

        public SpeedSystem()
        {
            _nodes = new Dictionary<int, (SpeedComponent speed, BodyComponent body)>();
            _speedComponentType = typeof(SpeedComponent);
            _bodyComponentType = typeof(BodyComponent);

            Entity.OnComponentAdded += HandleComponentAdded;
            Entity.OnComponentRemoved += HandleComponentRemoved;
        }

        public void Update()
        {
            foreach (var node in _nodes.Values)
            {
                // if we have reached our destination then we should do nothing
                if (node.speed.Destination == null) { continue; }

                var tx = new Flint(node.speed.Destination.Value.X) - node.body.Center.X;
                var ty = new Flint(node.speed.Destination.Value.Y) - node.body.Center.Y;
                var distance = FlintMath.Sqrt(tx * tx + ty * ty);

                if (distance > node.speed.Velocity)
                {
                    node.body.X += (tx / distance) * node.speed.Velocity;
                    node.body.Y += (ty / distance) * node.speed.Velocity;
                }
                else
                {
                    node.body.X = node.speed.Destination.Value.X - node.body.Width / 2;
                    node.body.Y = node.speed.Destination.Value.Y - node.body.Height / 2;
                    node.speed.Destination = null;
                }
            }
        }

        private void HandleComponentAdded(Entity entity, IComponent component)
        {
            HandleComponentRemoved(entity, component);

            if (entity.TryGetComponent<BodyComponent>(out var body)
                && entity.TryGetComponent<SpeedComponent>(out var speed))
            {
                _nodes[entity.Id] = (speed, body);
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