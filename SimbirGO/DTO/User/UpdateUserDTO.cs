namespace SimbirGO.DTO.User;

/// <summary>
/// Новая информация о пользователе
/// </summary>
public class UpdateUserDTO
{
    /// <summary> Роль пользователя в системе </summary>
    public Models.User.Roles? RoleOfUser { get; set; }
}