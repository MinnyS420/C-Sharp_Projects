#region Setup
using TaxiManagerApp9000.Domain.Entities;
using TaxiManagerApp9000.Domain.Enums;
using TaxiManagerApp9000.Helpers;
using TaxiManagerApp9000.Services;
using TaxiManagerApp9000.Services.Interfaces;

ICarService carService = new CarService();
IDriverService driverService = new DriverService();
IUserService userService = new UserService();
IUIService uiService = new UIService();
ILogger logger = new Logger();

InitializeStartingData();
#endregion

#region UI
while (true)
{
    Console.Clear();
    if (userService.CurrentUser == null)
    {
        try
        {
            User loginUser = uiService.LogIn();
            userService.Login(loginUser.Username, loginUser.Password);

            TextHelper.TextGenerator($"Successful Login! Welcome[{userService.CurrentUser.Role}] user!", ConsoleColor.Green);
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            logger.Log("Error", ex.Message, ex.StackTrace, "not logged in");
            TextHelper.TextGenerator(ex.Message, ConsoleColor.Red);
            Console.ReadLine();
            continue;
        }
    }

    bool loopActive = true;
    while (loopActive)
    {
        Console.Clear();
        int selectedItem = uiService.MainMenu(userService.CurrentUser.Role);
        if (selectedItem == -1)
        {
            TextHelper.TextGenerator("Wrong option Selected", ConsoleColor.Red);
            Console.ReadLine();
            continue;
        }
        MenuOptions choise = uiService.MenuChoice[selectedItem - 1];
        switch (choise)
        {
            case MenuOptions.AddNewUser:
                {
                    try
                    {
                        Console.Clear();
                        string username = TextHelper.GetInput("Username:");

                        // Validate the username using ValidationHelper
                        if (!ValidationHelper.ValidateUsername(username))
                        {
                            TextHelper.TextGenerator("Invalid username! Username must be between 5 and 20 characters and contain only letters, numbers, and underscores.", ConsoleColor.Red);
                            Console.ReadLine();
                            continue;
                        }

                        // Check if the username already exists
                        if (userService.UserExists(username))
                        {
                            TextHelper.TextGenerator("Username already exists! Please choose a different username.", ConsoleColor.Red);
                            Console.ReadLine();
                            continue;
                        }

                        string password = TextHelper.GetInput("Password:");

                        // Validate the password using ValidationHelper
                        if (!ValidationHelper.ValidatePassword(password))
                        {
                            TextHelper.TextGenerator("Invalid password! Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.", ConsoleColor.Red);
                            Console.ReadLine();
                            continue;
                        }

                        List<string> roles = new List<string>() { "Administrator", "Manager", "Maintenance" };
                        int enumInt = uiService.ChooseMenu(roles);

                        if (enumInt < 0 || enumInt > 2)
                        {
                            TextHelper.TextGenerator("Invalid role selection!", ConsoleColor.Red);
                            break;
                        }

                        Role role = (Role)enumInt;
                        User user = new User(username, password, role);
                        userService.Add(user);

                        TextHelper.TextGenerator("New User Added", ConsoleColor.Green);

                    }
                    catch (Exception ex)
                    {
                        TextHelper.TextGenerator(ex.Message, ConsoleColor.Red);
                    }
                    break;
                }
            case MenuOptions.RemoveExistingUser:
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("Select User For Removal (insert number in front of username):");
                        var usersForRemoval = userService.GetUsersForRemoval();
                        int selectedUser = uiService.ChooseEntityMenu(usersForRemoval);
                        if (selectedUser == -1)
                        {
                            TextHelper.TextGenerator("Wrong option Selected", ConsoleColor.Red);
                            continue;
                        }

                        if (userService.Remove(usersForRemoval[selectedUser - 1].Id))
                        {
                            TextHelper.TextGenerator("User removed", ConsoleColor.Yellow);
                        }
                    }
                    catch (Exception ex)
                    {
                        TextHelper.TextGenerator(ex.Message, ConsoleColor.Red);
                    }
                    break;
                }
            case MenuOptions.ListAllDrivers:
                {
                    Console.Clear();
                    var allData = driverService.GetAll();
                    AdvertisementHelper.DiscountAdd();
                    await allData;

                    foreach (var driver in allData.Result)
                    {
                        if (driver.CarId.HasValue)
                        {
                            driver.Car = carService.GetById(driver.CarId.Value);
                        }

                        Console.WriteLine(driver.Print());
                    }

                    Console.ReadLine();
                    break;
                }
            case MenuOptions.TaxiLicenseStatus:
                {
                    Console.Clear();
                    var allCars = carService.GetAll().Result;
                    var allDrivers = driverService.GetAll().Result;

                    foreach (var car in allCars)
                    {
                        var status = car.IsLicenseExpired();
                        var drivers = allDrivers.Where(driver => driver.CarId == car.Id).ToList();

                        switch (status)
                        {
                            case ExpiryStatus.Expired:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case ExpiryStatus.Valid:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case ExpiryStatus.Warning:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }

                        Console.WriteLine(car.Print());

                        foreach (var driver in drivers)
                        {
                            Console.WriteLine($"{driver.FirstName} {driver.LastName} with license {driver.License} expiring on {driver.LicenseExpiryDate.ToString("MM/yyyy")}");
                        }

                        Console.ResetColor();
                        Console.WriteLine();
                    }

                    Console.ReadLine();
                    break;
                }
            case MenuOptions.DriverManager:
                {
                    Console.Clear();
                    var driverManagerMenu = new List<DriverManagerChoice>()
                    {
                        DriverManagerChoice.AssignDriver,
                        DriverManagerChoice.UnassignDriver,
                        DriverManagerChoice.GoBack
                    };

                    int driverManagerChoice = uiService.ChooseMenu(driverManagerMenu);
                    var availableDrivers = driverService.GetAll(x => driverService.IsAvailableDriver(x));
                    foreach (var driver in availableDrivers)
                    {
                        if (driver.CarId.HasValue)
                        {
                            driver.Car = carService.GetById(driver.CarId.Value);
                        }
                    }

                    if (driverManagerChoice == 1)
                    {
                        Console.Clear();
                        var availableForAssigningDrivers = availableDrivers
                            .Where(x => x.CarId == null)
                            .ToList();
                        int assigningDrvierChoice = uiService
                            .ChooseEntityMenu<Driver>(availableForAssigningDrivers);
                        if (assigningDrvierChoice == -1) continue;

                        var availableCarsForAssigning = carService
                            .GetAll(x => carService.IsAvailableCar(x))
                            .ToList();
                        var assigningCarChoice = uiService
                            .ChooseEntityMenu<Car>(availableCarsForAssigning);
                        if (assigningCarChoice == -1) continue;

                        driverService.AssignDriver(
                            availableForAssigningDrivers[assigningDrvierChoice - 1],
                            availableCarsForAssigning[assigningCarChoice - 1]
                            );
                        carService.AssignDriver(availableForAssigningDrivers[assigningDrvierChoice - 1],
                            availableCarsForAssigning[assigningCarChoice - 1]
                            );
                    }
                    else if (driverManagerChoice == 2)
                    {
                        Console.Clear();
                        var availableForUnassigningDrivers = availableDrivers
                            .Where(x => x.CarId != null)
                            .ToList();
                        var unassigningDrvierChoice = uiService
                            .ChooseEntityMenu<Driver>(availableForUnassigningDrivers);
                        if (unassigningDrvierChoice == -1) continue;

                        driverService.Unassign(availableForUnassigningDrivers[unassigningDrvierChoice - 1]);
                    }
                    else if (driverManagerChoice == 3)
                    {
                        loopActive = false;
                        break;
                    }
                    break;
                }
            case MenuOptions.ListAllCars:
                {
                    Console.Clear();
                    var allCars = carService.GetAll().Result;
                    var allDrivers = driverService.GetAll().Result;

                    foreach (var car in allCars)
                    {
                        ExpiryStatus status = car.IsLicenseExpired();
                        ConsoleColor color;

                        switch (status)
                        {
                            case ExpiryStatus.Expired:
                                color = ConsoleColor.Red;
                                break;
                            case ExpiryStatus.Valid:
                                color = ConsoleColor.Green;
                                break;
                            case ExpiryStatus.Warning:
                                color = ConsoleColor.Yellow;
                                break;
                            default:
                                color = ConsoleColor.White;
                                break;
                        }

                        Console.ForegroundColor = color;
                        Console.WriteLine($"Car {car.Model} with plates {car.LicensePlate} that expire on {car.LicensePlateExpiryDate.ToString("M/yyyy")} is driven by:");

                        var drivers = allDrivers.Where(d => d.CarId == car.Id).ToList();
                        if (drivers.Any())
                        {
                            for (int i = 0; i < drivers.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}.) {drivers[i].FirstName} {drivers[i].LastName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No drivers assigned.");
                        }
                        Console.ResetColor();
                        Console.WriteLine();
                    }

                    Console.ReadLine();
                    break;
                }
            case MenuOptions.LicensePlateStatus:
                {
                    Console.Clear();
                    var allCars = carService.GetAll().Result;
                    var allDrivers = driverService.GetAll().Result;

                    foreach (var car in allCars)
                    {
                        ExpiryStatus status = car.IsLicenseExpired();
                        switch (status)
                        {
                            case ExpiryStatus.Expired:
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case ExpiryStatus.Valid:
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case ExpiryStatus.Warning:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }
                        Console.WriteLine($"Car Id: {car.Id} - Plate: {car.LicensePlate} with expiry date: {car.LicensePlateExpiryDate}");
                        Console.ResetColor();

                        Console.WriteLine("Drivers:");
                        var drivers = allDrivers.Where(d => d.CarId == car.Id).ToList();
                        if (drivers.Any())
                        {
                            for (int i = 0; i < drivers.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}.) {drivers[i].FirstName} {drivers[i].LastName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No drivers assigned.");
                        }
                        Console.WriteLine();
                    }

                    Console.ReadLine();
                    break;
                }
            case MenuOptions.ChangePassword:
                {
                    Console.Clear();
                    string oldPass = TextHelper.GetInput("Insert old password: ");
                    string newPass = TextHelper.GetInput("Insert new password: ");
                    if (userService.ChangePassword(userService.CurrentUser.Id, oldPass, newPass))
                    {
                        TextHelper.TextGenerator("Password changed!", ConsoleColor.Green);
                    }
                    break;
                }
            case MenuOptions.BackToMenu:
                {
                    userService.CurrentUser = null;
                    loopActive = false;
                    break;
                }
            case MenuOptions.Exit:
                {
                    userService.CurrentUser = null;
                    Environment.Exit(0);
                    break;
                }
        }
    }
}
#endregion

#region Seeding
void InitializeStartingData()
{
    User administrator = new User("BobBobsky", "bobbest1", Role.Administrator);
    User manager = new User("JillWayne", "jillawesome1", Role.Manager);
    User maintenances = new User("GregGregsky", "supergreg1", Role.Maintenance);
    List<User> seedUsers = new List<User>() { administrator, manager, maintenances };
    userService.Seed(seedUsers);

    Car car1 = new Car("Auris (Toyota)", "AFW950", new DateTime(2023, 12, 1));
    Car car2 = new Car("Auris (Toyota)", "CKE480", new DateTime(2021, 10, 15));
    Car car3 = new Car("Transporter (Volkswagen)", "GZDR69", new DateTime(2024, 5, 30));
    Car car4 = new Car("Mondeo (Ford)", "5RIP283", new DateTime(2022, 5, 13));
    Car car5 = new Car("Premier (Peugeot)", "2AR9907", new DateTime(2022, 11, 9));
    Car car6 = new Car("Vito (Mercedes)", "6RND294", new DateTime(2023, 3, 11));
    List<Car> seedCars = new List<Car>() { car1, car2, car3, car4, car5, car6 };
    carService.Seed(seedCars);

    Driver driver1 = new Driver("Romario", "Walsh", Shift.NoShift, null, "LC12456123", new DateTime(2023, 11, 5));
    Driver driver2 = new Driver("Kathleen", "Rankin", Shift.Morning, car1.Id, "LC54435234", new DateTime(2022, 1, 12));
    Driver driver3 = new Driver("Ashanti", "Mooney", Shift.Evening, car1.Id, "LC65803245", new DateTime(2022, 5, 19));
    Driver driver4 = new Driver("Zakk", "Hook", Shift.Afternoon, car1.Id, "LC20897583", new DateTime(2023, 9, 28));
    Driver driver5 = new Driver("Xavier", "Kelly", Shift.NoShift, null, "LC15636280", new DateTime(2024, 6, 1));
    Driver driver6 = new Driver("Joy", "Shelton", Shift.Evening, car2.Id, "LC47845611", new DateTime(2023, 7, 3));
    Driver driver7 = new Driver("Kristy", "Riddle", Shift.Morning, car3.Id, "LC19006543", new DateTime(2024, 6, 12));
    Driver driver8 = new Driver("Stuart", "Mayer", Shift.Evening, car3.Id, "LC53187767", new DateTime(2023, 10, 10));
    List<Driver> seedDrivers = new List<Driver>() { driver1, driver2, driver3, driver4, driver5, driver6, driver7, driver8 };
    driverService.Seed(seedDrivers);
}
#endregion