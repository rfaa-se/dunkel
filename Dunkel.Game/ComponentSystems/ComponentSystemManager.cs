using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.ComponentSystems
{
    public class ComponentSystemManager
    {
        private IUpdateComponentSystem[] _updateSystems;
        private IDrawComponentSystem[] _drawSystems;

        public ComponentSystemManager(IEnumerable<IUpdateComponentSystem> updateSystems, 
            IEnumerable<IDrawComponentSystem> drawSystems)
        {
            if (updateSystems == null) { updateSystems = new IUpdateComponentSystem[0]; }
            if (drawSystems == null) { drawSystems = new IDrawComponentSystem[0]; }

            if (updateSystems.Count() != updateSystems.Select(x => x.Priority).Distinct().Count())
            { throw new ArgumentException("two or more update component systems have the same priority"); }
            if (drawSystems.Count() != drawSystems.Select(x => x.Priority).Distinct().Count())
            { throw new ArgumentException("two or more draw component systems have the same priority"); }

            _updateSystems = updateSystems.OrderBy(x => x.Priority).ToArray();
            _drawSystems = drawSystems.OrderBy(x => x.Priority).ToArray();
        }

        public void Update()
        {
            foreach (var updateSystem in _updateSystems)
            {
                updateSystem.Update();
            }
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            foreach (var drawSystem in _drawSystems)
            {
                drawSystem.Draw(sb, delta);
            }
        }
    }
}