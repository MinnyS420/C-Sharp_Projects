using TimeTrackingApp.Domain.Entities;

namespace TimeTrackingApp.DataAccess.Interfaces
{
    public interface IDb<T> where T : BaseEntity
    {
        int Add(T user);
        T GetById(int id);
        T GetByUsername(string username);
        bool Update(T user);
        bool Delete(int id);
        List<T> GetAll();
    }
}
