namespace Core.Packages.Application.Security.Hashing;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
} 