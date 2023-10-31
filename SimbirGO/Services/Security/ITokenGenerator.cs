using Microsoft.IdentityModel.Tokens;
using SimbirGO.Models;

namespace SimbirGO.Services.Security;

/// <summary>
/// Интерфейс для генератора токена
/// </summary>
public interface ITokenGenerator
{
    SecurityToken GenerateToken(User user);
}
