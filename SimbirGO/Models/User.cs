namespace SimbirGO.Models;

public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Пароль, к которому применен хэш SHA-256
    /// </summary>
    public byte[]? PasswordHash { get; set; }

    /// <summary>
    /// Возможные роли в системе
    /// </summary>
    public enum Roles { User, Root }

    /// <summary>
    /// Роль пользователя в системе
    /// </summary>
    public Roles RoleOfUser { get; set; }
}
