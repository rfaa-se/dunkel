namespace Dunkel.Game.Components.Utilities
{
    public class CounterComponent : IComponent
    {
        public long Tick { get; set; }

        public void Reset()
        {
            Tick = 0;
        }
    }
}