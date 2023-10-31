namespace SimbirGO.Services.Security;

/// <summary>
/// Фабрика генераторов хеша с солью
/// </summary>
public class HashWithSaltGeneratorFactory
{
    /// <summary>
    /// Создает экземпляр <see cref="HashWithSaltGenerator"/>
    /// </summary>
    /// <param name="salt">Соль</param>
    /// <returns>Генератор хеша с солью</returns>
    public HashWithSaltGenerator Create(byte[] salt) => new(salt);
}