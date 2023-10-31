namespace SimbirGO.DTO.Auth;

/// <summary>
/// Данные необходимые для авторизации
/// </summary>
public record LoginUserDTO
{
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }
}
