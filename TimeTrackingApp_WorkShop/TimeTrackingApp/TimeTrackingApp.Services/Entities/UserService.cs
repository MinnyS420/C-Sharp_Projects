using TimeTrackingApp.DataAccess;
using TimeTrackingApp.Domain.Entities;
using TimeTrackingApp.Services.Entities.Interfaces;

namespace TimeTrackingApp.Services.Entities
{
    public class UserService : BaseService<User>, IUserService
    {
        protected new FileDataBase<User> Db;

        public UserService()
        {
            Db = new FileDataBase<User>();
        }

        public User? Login(string username, string password)
        {
            return GetAll().FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            User user = GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (user.Password == oldPassword)
            {
                user.Password = newPassword;
                return Db.Update(user); // Update user in the database
            }
            else
            {
                Console.WriteLine("Incorrect old password.");
                return false;
            }
        }

        public bool ChangeFirstName(int userId, string newFirstName)
        {
            User user = GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FirstName = newFirstName;
            return Db.Update(user); // Update user in the database
        }

        public bool ChangeLastName(int userId, string newLastName)
        {
            User user = GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.LastName = newLastName;
            return Db.Update(user); // Update user in the database
        }
    }
}
