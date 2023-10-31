using Microsoft.IdentityModel.Tokens;
using SimbirGO.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimbirGO.Services.Security;

/// <summary>
/// Генератор jwt токенов
/// </summary>
public class JWTTokenGenerator : ITokenGenerator
{
    private readonly JWTOptions _options;

    /// <summary>
    /// Создаёт экземпляр класс <see cref="JWTTokenGenerator"/>
    /// </summary>
    /// <param name="options">Конфигурация JWT в проекте</param>
    public JWTTokenGenerator(JWTOptions options) => _options = options;

    /// <summary>
    /// Метод генерации токена JWT 
    /// </summary>
    /// <param name="user">Данные пользователя</param>
    /// <returns>JWT токен</returns>
    public SecurityToken GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new(ClaimTypes.Role, user.RoleOfUser.ToString())
        };

        DateTime? lifetime = null;
        if (_options.Lifetime != null)
            lifetime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_options.Lifetime.Value));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: lifetime,
            signingCredentials: new SigningCredentials(_options.GetSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}
