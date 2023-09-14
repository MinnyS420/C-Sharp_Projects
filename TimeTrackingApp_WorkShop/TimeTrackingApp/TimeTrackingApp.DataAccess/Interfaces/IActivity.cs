using TimeTrackingApp.Domain.Entities;

namespace TimeTrackingApp.DataAccess.Interfaces
{
    public interface IActivity<T> where T : BaseEntity
    {
        int AddActivity(T activity);
        T GetActivityById(int id);
        bool UpdateActivity(T activity);
        bool DeleteActivity(int id);
        List<T> GetAllActivities();
    }
}
