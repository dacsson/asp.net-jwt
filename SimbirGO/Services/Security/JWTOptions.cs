using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SimbirGO.Services.Security
{
    /// <summary>
    /// Конфигурация авторизации
    /// </summary>
    public class JWTOptions
    {
        /// <summary>
        /// Кто предоставил токен
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Получатель токена
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Секретное значение
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Время жизни токена
        /// </summary>
        public double? Lifetime { get; set; }

        /// <summary>
        /// Создает симметричный ключ безопасности
        /// </summary>
        /// <returns>Симметричный ключ безопасности</returns>
        public SymmetricSecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
