using System;
using Dunkel.Game.Options;
using Microsoft.Extensions.Options;

namespace Dunkel.Game.Utilities
{
    public class FrameRater
    {
        public double Fps { get => (int)(_elapsedTimeMeasurements.Length / _totalElapsedTime); }

        private int _tick;
        private double[] _elapsedTimeMeasurements;
        private double _totalElapsedTime;

        public FrameRater(IOptions<EngineOptions> options)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));

            _elapsedTimeMeasurements = new double[options.Value.FrameRateSamples];
        }

        public void Update(double elapsedTimeSeconds)
        {
            if (++_tick >= _elapsedTimeMeasurements.Length) { _tick = 0; }

            _totalElapsedTime -= _elapsedTimeMeasurements[_tick];
            _totalElapsedTime += elapsedTimeSeconds;
            _elapsedTimeMeasurements[_tick] = elapsedTimeSeconds;
        }
    }
}