namespace SimbirGO.Services.Security;

/// <summary>
/// Интерфейс для генератора хеша
/// </summary>
public interface IHashGenerator
{
    byte[] GenerateHash(string password);
}
