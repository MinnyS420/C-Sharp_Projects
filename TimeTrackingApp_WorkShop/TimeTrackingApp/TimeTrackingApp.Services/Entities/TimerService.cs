using System.Diagnostics;
using TimeTrackingApp.Services.Entities.Interfaces;

namespace TimeTrackingApp.Services.Entities
{
    public class TimerService : ITimerService
    {
        private Stopwatch stopwatch;
        public event EventHandler TimerElapsed;
        public TimerService()
        {
            stopwatch = new Stopwatch();
        }
        public void Start()
        {
            stopwatch.Restart();
        }
        public void Stop()
        {
            stopwatch.Stop();
        }
        public int GetElapsedTime()
        {
            return (int)stopwatch.Elapsed.TotalSeconds;
        }
        public int GetTimeInSeconds()
        {
            int totalSeconds = GetElapsedTime();
            int seconds = totalSeconds;
            return seconds;
        }
        public int GetTimeInMinutes()
        {
            int totalSeconds = GetElapsedTime();
            int minutes = totalSeconds / 60;
            return minutes;
        }
        public int GetTimeInHours()
        {
            int totalSeconds = GetElapsedTime();
            int hours = totalSeconds / 3600; // 1 hour = 3600 seconds
            return hours;
        }
        public void TimerStartStop()
        {
            Start();
            Console.Clear();
            Console.WriteLine("Timer started. Press Enter to stop...");
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Stop();
                    break;
                }
            }
            int elapsedTime = GetElapsedTime();
            int minutes = GetTimeInMinutes();
            int seconds = elapsedTime % 60;
            Console.WriteLine($"Elapsed time: {minutes} minutes {seconds} seconds");
        }
    }
}