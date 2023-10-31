using System.Security.Cryptography;
using System.Text;

namespace SimbirGO.Services.Security;

/// <summary>
/// Генератор хэша с солью
/// </summary>
public class HashWithSaltGenerator : IHashGenerator
{
    /// <summary>
    /// Соль
    /// </summary>
    private readonly byte[] _salt;

    public HashWithSaltGenerator(byte[] salt) => _salt = salt;

    /// <summary>
    /// Метод генерации хэша с солью 
    /// </summary>
    /// <param name="password">Пароль, который нужно захэшировать</param>
    /// <returns>Хэш с солью</returns>
    public byte[] GenerateHash(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hash = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, _salt, 1200,
            HashAlgorithmName.SHA512, 64);
        return hash;
    }
}
