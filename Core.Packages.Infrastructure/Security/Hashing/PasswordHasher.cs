using System.Security.Cryptography;
using Core.Packages.Application.Security.Hashing;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Core.Packages.Infrastructure.Security.Hashing;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8; // 16 bytes
    private const int KeySize = 256 / 8;  // 32 bytes
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _hashAlgorithmName,
            KeySize
        );

        return string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash),
            Iterations,
            _hashAlgorithmName
        );
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            var elements = hashedPassword.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);
            var iterations = int.Parse(elements[2]);
            var algorithm = new HashAlgorithmName(elements[3]);

            var newHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(newHash, hash);
        }
        catch
        {
            return false;
        }
    }
} 