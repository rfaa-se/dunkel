using Dunkel.Game.Components;
using Dunkel.Game.Utilities;
using Microsoft.Xna.Framework;

namespace Dunkel.Game.Components.Attributes
{
    public class SpeedComponent : IComponent
    {
        public Flint Velocity { get; set; }
        public Point? Destination { get; set; }

        public void Reset()
        {
            Velocity = 0;
            Destination = null;
        }
    }
}