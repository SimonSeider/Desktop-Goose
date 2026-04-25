using System.Diagnostics;

namespace SamEngine
{
    public static class Time
    {
        public const int framerate = 120;

        public const float deltaTime = 0.008333334f;

        public static float time;

        static Time()
        {
            timeStopwatch.Start();
            TickTime();
        }

        public static void TickTime()
        {
            time = (float)timeStopwatch.Elapsed.TotalSeconds;
        }

        public static Stopwatch timeStopwatch = new Stopwatch();

    }
}
