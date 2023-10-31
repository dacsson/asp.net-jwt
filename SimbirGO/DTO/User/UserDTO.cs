namespace SimbirGO.DTO.User;

/// <summary>
/// Данные о пользователе
/// </summary>
public class UserDTO
{
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }

    /// <summary> 
    /// Роль пользователя в системе 
    /// </summary>
    public Models.User.Roles RoleOfUser { get; set; }
}
