using Newtonsoft.Json.Linq;
using SimbirGO.Data;
using SimbirGO.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// Интерфейс сервиса авторизации
/// </summary>
namespace SimbirGO.Services;

public interface IAuthService
{
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="username">Логин пользователя</param>
    /// <param name="password">пароль пользователя</param>
    /// <returns>Токен</returns>
    Token Authorize(string username, string password);

    /// <summary>
    /// Регистрация пользователя в системе
    /// </summary>
    /// <param name="user">данные профиля</param>
    /// <param name="role">Роль пользователя</param>
    Task Registrate(User user, User.Roles role, string password);

    /// <summary>
    /// Генерирует хэш
    /// </summary>
    /// <param name="password">пароль</param>
    /// <returns>Хэш</returns>
    byte[] GenerateHashes(string password);
}
