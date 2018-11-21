using System;
using System.Collections.Generic;
using System.Linq;
using Dunkel.Game.Components;
using Dunkel.Game.Components.Attributes;
using Dunkel.Game.Components.Graphics;
using Dunkel.Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.ComponentSystems.Draw
{
    public class DrawRectangleSystem : IDrawComponentSystem
    {
        public int Priority => 100;

        private readonly Dictionary<int, (PriorityTextureComponent draw, BodyComponent body)> _nodes;
        private readonly List<(PriorityTextureComponent draw, BodyComponent body)> _orderedNodes;
        private readonly Type _drawComponentType;
        private readonly Type _bodyComponentType;

        public DrawRectangleSystem()
        {
            _nodes = new Dictionary<int, (PriorityTextureComponent, BodyComponent)>();
            _orderedNodes = new List<(PriorityTextureComponent draw, BodyComponent body)>();
            _drawComponentType = typeof(PriorityTextureComponent);
            _bodyComponentType = typeof(BodyComponent);

            Entity.OnComponentAdded += HandleComponentAdded;
            Entity.OnComponentRemoved += HandleComponentRemoved;
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            foreach (var node in _orderedNodes)
            {
                var draw = node.draw;
                var body = node.body;

                sb.DrawRectangle(
                    x: MathHelper.Lerp(body.PrevX, body.X, delta),
                    y: MathHelper.Lerp(body.PrevY, body.Y, delta),
                    width: MathHelper.Lerp(body.PrevWidth, body.Width, delta),
                    height: MathHelper.Lerp(body.PrevHeight, body.Height, delta),
                    color: Color.White,
                    rotation: MathHelper.ToRadians(MathHelper.Lerp(body.PrevRotation, body.Rotation, delta))
                );
            }
        }

        private void HandleComponentAdded(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);

            if (   entity.TryGetComponent<PriorityTextureComponent>(out var draw)
                && entity.TryGetComponent<BodyComponent>(out var body))
            {
                _nodes[entity.Id] = (draw, body);
                // TODO: might have to optimize this
                _orderedNodes.Clear();
                _orderedNodes.AddRange(_nodes
                    .OrderBy(x => x.Value.draw.Priority)
                    .ThenBy(x => x.Key)
                    .Select(x => x.Value));
            }
        }

        private void HandleComponentRemoved(Entity entity, IComponent component)
        {
            if (!IsRelevantComponent(component)) { return; }

            _nodes.Remove(entity.Id);
            // TODO: might have to optimize this
            _orderedNodes.Clear();
            _orderedNodes.AddRange(_nodes
                .OrderBy(x => x.Value.draw.Priority)
                .ThenBy(x => x.Key)
                .Select(x => x.Value));
        }

        private bool IsRelevantComponent(IComponent component)
        {
            var type = component.GetType();

            return type == _drawComponentType || type == _bodyComponentType;
        }
    }
}