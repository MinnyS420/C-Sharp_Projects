namespace TimeTrackingApp.Services.Entities.Interfaces
{
    public interface ITimerService
    {
        event EventHandler TimerElapsed;
        void Start();
        void Stop();
        int GetTimeInMinutes();
        int GetElapsedTime();
    }
}
