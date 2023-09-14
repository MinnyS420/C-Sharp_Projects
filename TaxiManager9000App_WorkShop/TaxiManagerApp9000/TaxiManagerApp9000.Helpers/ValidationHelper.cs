using System.Text.RegularExpressions;

public static class ValidationHelper
{
    public static bool ValidateUsername(string username)
    {
        // Check length
        if (username.Length < 5 || username.Length > 20)
        {
            return false;
        }

        // Check character set (alphanumeric and underscores)
        if (!Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
        {
            return false;
        }

        // Ensure username doesn't start or end with an underscore
        if (username.StartsWith("_") || username.EndsWith("_"))
        {
            return false;
        }

        // Add more checks as needed

        return true;
    }

    public static bool ValidatePassword(string password)
    {
        // Check length
        if (password.Length < 8)
        {
            return false;
        }

        // Check complexity (at least one uppercase, one lowercase, one digit, and one special character)
        if (!Regex.IsMatch(password, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]+$"))
        {
            return false;
        }

        // Ensure password doesn't contain the word "password" (case-insensitive)
        if (password.ToLower().Contains("password"))
        {
            return false;
        }

        // Add more checks as needed

        return true;
    }
}
