using TimeTrackingApp.Domain.Entities;

namespace TimeTrackingApp.Services.Entities.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity
    {
        bool Add(T user);
        T GetById(int id);
        T GetByUsername(string username);
        bool Delete(int id);
        List<T> GetAll();
    }
}
