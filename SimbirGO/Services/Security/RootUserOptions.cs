namespace SimbirGO.Services.Security;

/// <summary>
/// Конфигурация root пользователя
/// </summary>
public class RootUserOptions
{
    /// <summary>
    /// Электронная почта пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество пользователя
    /// </summary>
    public string Patronymic { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }
}
