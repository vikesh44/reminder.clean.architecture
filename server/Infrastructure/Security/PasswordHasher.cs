
using System.Security.Cryptography;
using System.Text;
using ReminderApp.Application.Ports;

namespace ReminderApp.Infrastructure.Security;

// Simple PBKDF2 password hasher
public class PasswordHasher : IPasswordHasher
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    public string Hash(string plain)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, Iterations, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(KeySize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }

    public bool Verify(string plain, string hash)
    {
        var parts = hash.Split('.');
        if (parts.Length != 3) return false;
        var iterations = int.Parse(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);
        using var pbkdf2 = new Rfc2898DeriveBytes(plain, salt, iterations, HashAlgorithmName.SHA256);
        var computed = pbkdf2.GetBytes(key.Length);
        return CryptographicOperations.FixedTimeEquals(computed, key);
    }
}
