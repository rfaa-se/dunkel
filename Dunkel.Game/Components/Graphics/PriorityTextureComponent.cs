using Dunkel.Game.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.Components.Graphics
{
    public class PriorityTextureComponent : IComponent
    {
        public Texture2D Texture { get; set; }
        public int Priority { get; set; }

        public void Reset()
        {
            Priority = 0;
            Texture = null;
        }
    }
}