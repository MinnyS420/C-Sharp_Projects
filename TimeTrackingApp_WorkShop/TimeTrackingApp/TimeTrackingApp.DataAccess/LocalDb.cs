using TimeTrackingApp.DataAccess.Interfaces;
using TimeTrackingApp.Domain.Entities;

namespace TimeTrackingApp.DataAccess
{
    public class LocalDb<T> : IDb<T> where T : BaseEntity
    {
        public int IdCounter { get; set; }

        private List<T> db;
        public LocalDb()
        {
            db = new List<T>();
            IdCounter = 1;
        }

        public List<T> GetAll()
        {
            return db;
        }


        public User Authenticate(string username, string password)
        {
            return db.OfType<User>().FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public int Add(T user)
        {
            user.Id = IdCounter++;
            db.Add(user);
            return user.Id;
        }

        public T GetById(int id)
        {
            return db.Single(x => x.Id == id);
        }

        public T GetByUsername(string username)
        {
            return db.Single(x => x.Username == username);

        }

        public bool Update(T user)
        {
            try
            {
                T dbUser = GetById(user.Id);

                dbUser = user;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                T user = GetById(id);
                db.Remove(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
