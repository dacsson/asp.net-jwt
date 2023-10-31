namespace SimbirGO.DTO.Auth;

/// <summary>
/// JWT токен
/// </summary>
public record UserAccessTokenDTO
{
    /// <summary>
    /// Access token
    /// </summary>
    public string Token { get; set; }
}
