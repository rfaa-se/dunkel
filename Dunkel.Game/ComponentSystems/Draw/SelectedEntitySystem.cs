using System;
using System.Collections.Generic;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Components.Graphics;
using Dunkel.Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.ComponentSystems.Draw
{
    public class SelectedEntitySystem : IDrawComponentSystem
    {
        public int Priority => 99;

        private readonly Dictionary<int, (SelectedEntityComponent selected, BodyComponent body)> _nodes;
        private readonly Type _selectedEntityComponentType;
        private readonly Type _bodyComponentType;

        public SelectedEntitySystem()
        {
            _nodes = new Dictionary<int, (SelectedEntityComponent, BodyComponent)>();
            _selectedEntityComponentType = typeof(SelectedEntityComponent);
            _bodyComponentType = typeof(BodyComponent);

            Entity.OnComponentAdded += HandleComponentAdded;
            Entity.OnComponentRemoved += HandleComponentRemoved;
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            foreach (var node in _nodes)
            {
                var body = node.Value.body;
                
                sb.DrawRectangle(
                    x: MathHelper.Lerp(body.PrevX - 2, body.X - 2, delta),
                    y: MathHelper.Lerp(body.PrevY - 2, body.Y - 2, delta),
                    width: MathHelper.Lerp(body.PrevWidth + 4, body.Width + 4, delta),
                    height: MathHelper.Lerp(body.PrevHeight + 4, body.Height + 4, delta),
                    color: Color.Red,
                    rotation: MathHelper.ToRadians(MathHelper.Lerp(body.PrevRotation, body.Rotation, delta))
                );
            }
        }

        private void HandleComponentAdded(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);

            if (   entity.TryGetComponent<SelectedEntityComponent>(out var selected)
                && entity.TryGetComponent<BodyComponent>(out var body))
            {
                _nodes[entity.Id] = (selected, body);
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

            return type == _selectedEntityComponentType || type == _bodyComponentType;
        }
    }
}