namespace Dunkel.Game.Components.Utilities
{
    public class PlayerIdComponent : IComponent
    {
        public long PlayerId { get; set; }

        public void Reset()
        {
            PlayerId = 0;
        }
    }
}