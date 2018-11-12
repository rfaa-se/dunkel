using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Entities;

namespace Dunkel.Game.ComponentSystems.Update
{
    public class MovementSystem : IUpdateComponentSystem
    {
        public int Priority => 20;
        
        private readonly Dictionary<int, (SpeedComponent speed, BodyComponent body, Counter counter)> _nodes;
        private readonly Type _speedComponentType;
        private readonly Type _bodyComponentType;

        public MovementSystem()
        {
            _nodes = new Dictionary<int, (SpeedComponent, BodyComponent, Counter)>();
            _speedComponentType = typeof(SpeedComponent);
            _bodyComponentType = typeof(BodyComponent);

            Entity.OnComponentAdded += HandleComponentAdded;
            Entity.OnComponentRemoved += HandleComponentRemoved;
        }

        public void Update()
        {
            foreach (var node in _nodes.Values)
            {
                if (node.counter.Tick == 16*5*4) { node.counter.Tick = 0; }

                if (node.counter.Tick < 16 * 5)
                {
                    node.body.X += node.speed.Velocity;
                }
                else if (node.counter.Tick < 16 * 5 * 2)
                {
                    node.body.Y += node.speed.Velocity;
                }
                else if (node.counter.Tick < 16 * 5 * 3)
                {
                    node.body.X -= node.speed.Velocity;
                }
                else
                {
                    node.body.Y -= node.speed.Velocity;
                }

                node.counter.Tick++;
            }
        }

        private void HandleComponentAdded(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);

            var speed = entity.GetComponent<SpeedComponent>();
            var body = entity.GetComponent<BodyComponent>();

            if (speed == null || body == null) { return; }

            _nodes[entity.Id] = (speed, body, new Counter());
        }

        private void HandleComponentRemoved(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);
        }

        private bool IsRelevantComponent(IComponent component)
        {
            var type = component.GetType();

            return type == _speedComponentType || type == _bodyComponentType;
        }

        private class Counter
        {
            public int Tick { get; set; }
        }
    }
}