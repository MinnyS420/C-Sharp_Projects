using TimeTrackingApp.Domain.Entities;

namespace TimeTrackingApp.Services.Entities.Interfaces
{
    public interface IUserService
    {
        User? Login(string username, string password);
        bool ChangePassword(int userId, string oldPassword, string newPassword);
    }
}