using TimeTrackingApp.Domain.Entities;

namespace TimeTrackingApp.Services.Entities.Interfaces
{
    public interface IUIService
    {
        User LoginMenu();
        void RegisterMenu(UserService userService);
        void MainMenu(UserService userService);
        void UserMenu(User loggedInUser, UserService userService);
        void TrackingMenu(User loggedInUser);
        void UserStatisticsMenu(User loggedInUser);
        void AccountManagementMenu(User loggedInUser);
    }
}
