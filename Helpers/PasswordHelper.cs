using System.Security.Cryptography;
using TriviaNight.Interfaces;

namespace TriviaNight.Helpers;

public class PasswordHelper : IPasswordHelper
{
    private const int saltSize = 16;
    private const int hashSize = 32;
    private const int iterations = 100000;

    private readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, algorithm, hashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

}
