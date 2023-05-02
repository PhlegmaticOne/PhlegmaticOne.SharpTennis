using System;
using System.Diagnostics;

namespace PhlegmaticOne.SharpTennis.Game.Common.Infrastructure
{
    public static class Time
    {
        private static readonly Stopwatch Stopwatch;
        private static int _framesCounter;
        private static int _fps;
        private static long _previousFpsMeasurementTime;
        private static long _previousTicks;
        private static float _time;
        private static float _deltaT;
        public static int Fps => _fps;
        public static float PassedTime => _time;
        public static float DeltaT => _deltaT;
        public static bool Paused { get; set; }

        public static event Action Updated;
        static Time()
        {
            Stopwatch = new Stopwatch();
            Reset();
        }


        public static void Reset()
        {
            Stopwatch.Reset();
            _framesCounter = 0;
            _fps = 0;
            Stopwatch.Start();
            _previousFpsMeasurementTime = Stopwatch.ElapsedMilliseconds;
            _previousTicks = Stopwatch.ElapsedTicks;
        }

        public static void Update()
        {
            var ticks = Stopwatch.ElapsedTicks;
            _time = (float)ticks / TimeSpan.TicksPerSecond;
            _deltaT = (float)(ticks - _previousTicks) / TimeSpan.TicksPerSecond;
            _previousTicks = ticks;

            _framesCounter++;
            if (Stopwatch.ElapsedMilliseconds - _previousFpsMeasurementTime >= 1000)
            {
                _fps = _framesCounter;
                _framesCounter = 0;
                _previousFpsMeasurementTime = Stopwatch.ElapsedMilliseconds;
            }

            if (Paused)
            {
                return;
            }

            Updated?.Invoke();
        }
    }
}
