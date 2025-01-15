using System.Security.Cryptography;
using System.Text;

namespace Core.Packages.Security.Helpers;

public static class HashingHelper
{
    public static string CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hash = hmac.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPasswordHash(string password, string storedHash)
    {
        using var hmac = new HMACSHA512();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var computedHash = hmac.ComputeHash(passwordBytes);
        var storedHashBytes = Convert.FromBase64String(storedHash);
        
        return computedHash.SequenceEqual(storedHashBytes);
    }
} 