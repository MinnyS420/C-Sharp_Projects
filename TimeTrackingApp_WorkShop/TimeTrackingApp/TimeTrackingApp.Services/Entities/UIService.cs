using TimeTrackingApp.Domain.Entities;
using TimeTrackingApp.Domain.Enums;
using TimeTrackingApp.Helpers;
using TimeTrackingApp.Services.Entities.Interfaces;
namespace TimeTrackingApp.Services.Entities
{
    public class UIService : IUIService
    {
        private readonly UserService userService = new UserService();
        //public FileDataBase<User> dataBase = new FileDataBase<User>();

        List<User> users = new List<User> { };  //Username: admin123 Password:123456 Username: admin234 Password:123456
        List<Exercising> exercoses = new List<Exercising> { };
        List<Hobbies> hobbies = new List<Hobbies> { };
        List<Reading> readings = new List<Reading> { };
        List<Working> infos = new List<Working> { };

        public void InitializeData()
        {
            foreach (User user in users)
            {
                userService.Add(user);
            }
        }
        public User LoginMenu()
        {
            const int maxAttempts = 3;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                Console.Clear();
                TextHelper.TextGenerator("Log in to the app", ConsoleColor.DarkMagenta);
                TextHelper.TextGenerator("\nEnter username: ", ConsoleColor.DarkMagenta);
                string usernameInput = Console.ReadLine();

                if (string.IsNullOrEmpty(usernameInput))
                {
                    TextHelper.TextGenerator("Invalid input!", ConsoleColor.Red);
                    Console.ReadKey();
                    continue;
                }

                TextHelper.TextGenerator("Enter password: ", ConsoleColor.DarkMagenta);
                string passwordInput = Console.ReadLine();

                if (string.IsNullOrEmpty(passwordInput))
                {
                    TextHelper.TextGenerator("Invalid input!", ConsoleColor.Red);
                    Console.ReadKey();
                    continue;
                }

                User loggedInUser = userService.Login(usernameInput, passwordInput);

                if (loggedInUser == null)
                {
                    attempts++;
                    TextHelper.TextGenerator($"Invalid username or password. You have {maxAttempts - attempts} attempts left.", ConsoleColor.Yellow);
                    Console.ReadKey();

                    if (attempts == maxAttempts)
                    {
                        TextHelper.TextGenerator($"You have exceeded the maximum number of login attempts. Goodbye!", ConsoleColor.Red);
                        Environment.Exit(0);
                    }

                    TextHelper.TextGenerator($"Press any key to try again. Enter N to go back to the menu.", ConsoleColor.Yellow);
                    string decide = Console.ReadLine();
                    if (decide == "N")
                    {
                        MainMenu(userService);
                    }
                    continue;
                }
                UserMenu(loggedInUser, userService);
                return loggedInUser;
            }
            TextHelper.TextGenerator($"You have exceeded the maximum number of login attempts. Goodbye!", ConsoleColor.Red);
            Environment.Exit(0);
            return null;
        }
        public void MainMenu(UserService userService)
        {
            while (true)
            {
                Console.Clear();
                TextHelper.TextGenerator("Welcome to Time Tracking App ⌛", ConsoleColor.Magenta);
                TextHelper.TextGenerator("Please choose one:", ConsoleColor.Magenta);
                TextHelper.TextGenerator("1). Login", ConsoleColor.Magenta);
                TextHelper.TextGenerator("2). Register", ConsoleColor.Magenta);
                TextHelper.TextGenerator("3). Exit", ConsoleColor.Magenta);
                int choise = inputValidation();

                switch (choise)
                {
                    case 1:
                        LoginMenu();
                        continue;

                    case 2:
                        RegisterMenu(userService);
                        continue;

                    case 3:
                        TextHelper.TextGenerator("Thx for using this app! Have a nice day/night!\nGoodbye!", ConsoleColor.Green);
                        Environment.Exit(0);
                        break;

                    default:
                        TextHelper.TextGenerator("Something went wrong!", ConsoleColor.Red);
                        MainMenu(userService);
                        break;
                }
            }
        }
        public void RegisterMenu(UserService userService)
        {
            bool ContainsNumber(string s)
            {
                return s.Any(char.IsDigit);
            }

            string firstName = "";
            string lastName = "";
            int age = 0;
            string username = "";
            string password = "";

            while (firstName.Length < 2)
            {
                Console.Clear();
                TextHelper.TextGenerator("The First Name must be longer than 2! ", ConsoleColor.Yellow);
                TextHelper.TextGenerator("First Name: ", ConsoleColor.Green);
                firstName = Console.ReadLine();

                if (firstName.Length < 2)
                {
                    TextHelper.TextGenerator("Creation unsuccessful. Your first name must have more than 2 letters. Please try again", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }

            while (lastName.Length < 2)
            {
                Console.Clear();
                TextHelper.TextGenerator("The Last Name must be longer than 2! ", ConsoleColor.Yellow);
                TextHelper.TextGenerator("Last Name: ", ConsoleColor.Green);
                lastName = Console.ReadLine();

                if (lastName.Length < 2)
                {
                    TextHelper.TextGenerator("Creation unsuccessful. Your last name must have more than 2 letters. Please try again", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }

            while (age < 18 || age > 120)
            {
                Console.Clear();
                TextHelper.TextGenerator("Must be older than 18! User above 120 years can't register! ", ConsoleColor.Yellow);
                TextHelper.TextGenerator("Age: ", ConsoleColor.Green);

                if (!int.TryParse(Console.ReadLine(), out age))
                {
                    TextHelper.TextGenerator("Invalid input. Please enter a valid age.", ConsoleColor.Red);
                    Console.ReadKey();
                    continue;
                }

                if (age < 18 || age > 120)
                {
                    TextHelper.TextGenerator("Creation unsuccessful. Please try again", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }

            while (username.Length < 5)
            {
                Console.Clear();
                TextHelper.TextGenerator("The Username must be longer than 5! ", ConsoleColor.Yellow);
                TextHelper.TextGenerator("Username: ", ConsoleColor.Green);
                username = Console.ReadLine();

                if (username.Length < 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TextHelper.TextGenerator("Creation unsuccessful. Username must have more than 5 characters. Please try again", ConsoleColor.Red);
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            while (password.Length < 6 || !ContainsNumber(password) || !password.Any(char.IsUpper))
            {
                Console.Clear();
                TextHelper.TextGenerator("The Password must be longer than 6, must contain 1 uppercase letter, and at least 1 number!  ", ConsoleColor.Yellow);
                TextHelper.TextGenerator("Password: ", ConsoleColor.Green);
                password = Console.ReadLine();

                if (password.Length < 6 || !ContainsNumber(password) || !password.Any(char.IsUpper))
                {
                    TextHelper.TextGenerator("Creation unsuccessful. Password must be at least 6 characters long, contain at least one number, and have at least one uppercase letter. Please try again.", ConsoleColor.Red);
                    Console.ReadKey();
                }
            }

            User newUser = new User(firstName, lastName, age, username, password);
            userService.Add(newUser);
            TextHelper.TextGenerator($"Successful creation of your account!", ConsoleColor.Green);
            Console.ReadKey();
        }

        public void UserMenu(User loggedInUser, UserService userService)
        {
            while (true)
            {
                Console.Clear();
                TextHelper.TextGenerator("Welcome to Time Tracking App ⌛", ConsoleColor.Magenta);
                TextHelper.TextGenerator("Please choose one:", ConsoleColor.Magenta);
                TextHelper.TextGenerator("1). Logout", ConsoleColor.Magenta);
                TextHelper.TextGenerator("2). Tracking", ConsoleColor.Magenta);
                TextHelper.TextGenerator("3). User Statistics", ConsoleColor.Magenta);
                TextHelper.TextGenerator("4). Account Management", ConsoleColor.Magenta);
                int choise = inputValidation();



                switch (choise)
                {
                    case 1:
                        TextHelper.TextGenerator($"Goodbye {loggedInUser.FirstName} {loggedInUser.LastName}!", ConsoleColor.Green);
                        Console.ReadKey();
                        MainMenu(userService);
                        continue;

                    case 2:
                        TrackingMenu(loggedInUser);
                        continue;

                    case 3:
                        UserStatisticsMenu(loggedInUser);
                        continue;

                    case 4:
                        AccountManagementMenu(loggedInUser);
                        continue;

                    default:
                        TextHelper.TextGenerator("Something went wrong!", ConsoleColor.Red);
                        Environment.Exit(0);
                        break;
                }
            }
        }
        public void TrackingMenu(User loggedInUser)
        {
            TimerService timerService = new TimerService();
            while (true)
            {
                Console.Clear();
                TextHelper.TextGenerator("Select Activity:", ConsoleColor.Magenta);
                TextHelper.TextGenerator($"1).Exercising", ConsoleColor.Magenta);
                TextHelper.TextGenerator($"2).Reading", ConsoleColor.Magenta);
                TextHelper.TextGenerator($"3).Working", ConsoleColor.Magenta);
                TextHelper.TextGenerator("4).Back to Main Menu", ConsoleColor.Magenta);
                TextHelper.TextGenerator("5).Exit", ConsoleColor.Magenta);
                int choise = inputValidation();
                switch (choise)
                {
                    case 1:
                        Console.Clear();
                        TextHelper.TextGenerator("Exercising Options:", ConsoleColor.Magenta);
                        TextHelper.TextGenerator("1). Start timer", ConsoleColor.Magenta);
                        TextHelper.TextGenerator("2). Back to menu", ConsoleColor.Magenta);
                        int exerciseChoice = inputValidation();

                        switch (exerciseChoice)
                        {
                            case 1:
                                timerService.TimerStartStop();
                                int duration = timerService.GetTimeInSeconds();
                                // Choose the activity type
                                TextHelper.TextGenerator("Choose the type of activity:", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"1). {EExercising.Global.ToString()}", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"2). {EExercising.Running.ToString()}", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"3). {EExercising.Sport.ToString()}", ConsoleColor.Magenta);
                                int activityTypeChoice = inputValidation();
                                EExercising chosenActivityType = (EExercising)activityTypeChoice;

                                // Save the activity type for later use
                                // You can implement your saving logic here
                                break;

                            case 2:
                                TrackingMenu(loggedInUser);
                                break;

                            default:
                                TextHelper.TextGenerator("Invalid choice!", ConsoleColor.Red);
                                break;
                        }
                        continue;

                    case 2:
                        Console.Clear();
                        TextHelper.TextGenerator("Reading Options:", ConsoleColor.Magenta);
                        TextHelper.TextGenerator("1). Start timer", ConsoleColor.Magenta);
                        TextHelper.TextGenerator("2). Back to menu", ConsoleColor.Magenta);
                        int exerciseChoice1 = inputValidation();

                        switch (exerciseChoice1)
                        {
                            case 1:
                                timerService.TimerStartStop();
                                int duration = timerService.GetTimeInSeconds();

                                TextHelper.TextGenerator("Choose the type of activity:", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"1). {EReading.Belles_Lettres.ToString()}", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"2). {EReading.Fiction.ToString()}", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"3). {EReading.Professional_Literature.ToString()}", ConsoleColor.Magenta);
                                int activityTypeChoice = inputValidation();
                                EReading chosenActivityType = (EReading)activityTypeChoice;

                                int readPages;

                                while (true)
                                {
                                    TextHelper.TextGenerator("\nEnter the number of read pages:", ConsoleColor.Magenta);
                                    readPages = inputValidation();

                                    if (readPages != -1)
                                    {
                                        // The input is a valid number
                                        break;
                                    }
                                    else
                                    {
                                        // The input is not a valid number
                                        TextHelper.TextGenerator("Invalid input. Please enter a number.", ConsoleColor.Red);
                                    }
                                }
                                // Save the activity type for later use
                                // You can implement your saving logic here
                                break;
                            case 2:
                                TrackingMenu(loggedInUser);
                                break;

                        }
                        continue;

                    case 3:
                        Console.Clear();

                        TextHelper.TextGenerator("Working Options:", ConsoleColor.Magenta);
                        TextHelper.TextGenerator("1). Start timer", ConsoleColor.Magenta);
                        TextHelper.TextGenerator("2). Back to menu", ConsoleColor.Magenta);
                        int exerciseChoice2 = inputValidation();
                        switch (exerciseChoice2)
                        {
                            case 1:
                                timerService.TimerStartStop();
                                int duration = timerService.GetTimeInSeconds();

                                TextHelper.TextGenerator("Choose the type of activity:", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"1). {EWorking.Office.ToString()}", ConsoleColor.Magenta);
                                TextHelper.TextGenerator($"2). {EWorking.Home.ToString()}", ConsoleColor.Magenta);
                                int activityTypeChoice = inputValidation();
                                EWorking chosenActivityType = (EWorking)activityTypeChoice;

                                // Save the activity type for later use
                                // You can implement your saving logic here
                                break;
                            case 2:
                                TrackingMenu(loggedInUser);
                                break;

                        }
                        continue;

                    case 4:
                        UserMenu(loggedInUser, userService);
                        break;

                    case 5:
                        TextHelper.TextGenerator("Thx for using this app! Have a nice day/night!\nGoodbye!", ConsoleColor.Green);
                        Environment.Exit(0);
                        break;

                    default:
                        TextHelper.TextGenerator("Something went wrong!", ConsoleColor.Red);
                        Environment.Exit(0);
                        break;
                }
            }

        }

        public void UserStatisticsMenu(User loggedInUser)
        {
            throw new NotImplementedException();
        }
        public void AccountManagementMenu(User loggedInUser)
        {
            while (true)
            {
                Console.Clear();
                TextHelper.TextGenerator("**Account Management**", ConsoleColor.Magenta);
                TextHelper.TextGenerator("1). Change Password ", ConsoleColor.Magenta);
                TextHelper.TextGenerator("2). Change FirstName", ConsoleColor.Magenta);
                TextHelper.TextGenerator("3). Change LastName", ConsoleColor.Magenta);
                TextHelper.TextGenerator("4). Back to menu", ConsoleColor.Magenta);
                int choice = inputValidation();

                switch (choice)
                {
                    case 1:
                        TextHelper.TextGenerator("Enter the old password: ", ConsoleColor.Magenta);
                        string oldPassword = Console.ReadLine();
                        TextHelper.TextGenerator("Enter the new password: ", ConsoleColor.Green);
                        string newPassword = Console.ReadLine();

                        if (userService.ChangePassword(loggedInUser.Id, oldPassword, newPassword))
                        {
                            TextHelper.TextGenerator("Password changed successfully.", ConsoleColor.Green);
                        }
                        else
                        {
                            TextHelper.TextGenerator("Failed to change the password.", ConsoleColor.Red);
                        }

                        Console.ReadKey();
                        break;

                    case 2:
                        TextHelper.TextGenerator("Enter the new first name: ", ConsoleColor.Magenta);
                        string newFirstName = Console.ReadLine();

                        if (userService.ChangeFirstName(loggedInUser.Id, newFirstName))
                        {
                            TextHelper.TextGenerator("First name changed successfully.", ConsoleColor.Green);
                        }
                        else
                        {
                            TextHelper.TextGenerator("Failed to change the first name.", ConsoleColor.Red);
                        }

                        Console.ReadKey();
                        break;

                    case 3:
                        TextHelper.TextGenerator("Enter the new last name: ", ConsoleColor.Magenta);
                        string newLastName = Console.ReadLine();

                        if (userService.ChangeLastName(loggedInUser.Id, newLastName))
                        {
                            TextHelper.TextGenerator("Last name changed successfully.", ConsoleColor.Green);
                        }
                        else
                        {
                            TextHelper.TextGenerator("Failed to change the last name.", ConsoleColor.Red);
                        }

                        Console.ReadKey();
                        break;

                    case 4:
                        UserMenu(loggedInUser, userService);
                        return;

                    default:
                        TextHelper.TextGenerator("Invalid choice. Please try again.", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }
        public static int inputValidation()
        {
            string input = Console.ReadLine();
            bool inputValidation = int.TryParse(input, out int choice);

            if (!inputValidation)
            {
                TextHelper.TextGenerator("Invalid input, try again", ConsoleColor.Red);
                Console.ReadKey();
            }

            return choice;
        }
    }
}