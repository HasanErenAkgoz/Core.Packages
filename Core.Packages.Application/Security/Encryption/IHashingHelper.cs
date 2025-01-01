namespace Core.Packages.Application.Security.Encryption;

public interface IHashingHelper
{
    void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);
    bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt);
} 