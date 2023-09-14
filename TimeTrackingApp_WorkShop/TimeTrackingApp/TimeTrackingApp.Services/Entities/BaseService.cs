using TimeTrackingApp.DataAccess;
using TimeTrackingApp.DataAccess.Interfaces;
using TimeTrackingApp.Domain.Entities;
using TimeTrackingApp.Services.Entities.Interfaces;

namespace TimeTrackingApp.Services.Entities
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private IDb<T> Db;
        public BaseService()
        {
            Db = new FileDataBase<T>();
        }

        public bool Add(T user)
        {
            try
            {
                Db.Add(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            return Db.Delete(id);
        }

        public List<T> GetAll()
        {
            return Db.GetAll();
        }

        public T GetById(int id)
        {
            return Db.GetById(id);
        }

        public T GetByUsername(string username)
        {
            return Db.GetByUsername(username);

        }
    }
}
