using Dunkel.Game.Components;
using Dunkel.Game.Utilities;

namespace Dunkel.Game.Components.Attributes
{
    public class SpeedComponent : IComponent
    {
        public Flint Velocity { get; set; }

        public void Reset()
        {
            Velocity = 0;
        }
    }
}