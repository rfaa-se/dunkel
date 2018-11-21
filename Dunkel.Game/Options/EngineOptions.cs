namespace Dunkel.Game.Options
{
    public class EngineOptions
    {
        public int TicksPerSecond { get; set; } = 16;
        public int FrameRateSamples { get; set; } = 10;
        public int TicksFutureSchedule { get; set; } = 3;
    }
}