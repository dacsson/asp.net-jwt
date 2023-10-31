using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimbirGO.Data;
using SimbirGO.DTO.Auth;
using SimbirGO.Models;
using SimbirGO.Services;

namespace SimbirGO.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authorizationService;

    /// <summary>
    /// Создание экземпляра класса <see cref="AuthController"/>
    /// </summary>
    /// <param name="authorizationService">Экземляр сервиса авторизации</param>
    public AuthController(IAuthService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Авторизация в системе
    /// </summary>
    /// <param name="loginUserDto">Необходимые данные для авторизации пользователя в системе</param>
    /// <returns>JWT токен</returns>
    /// <exception cref="NotFoundException{T}"><see cref="DTO.Authorization.ErrorCode.AccountNotFound"/></exception>
    /// <exception cref="ForbiddenException{T}"> <see cref="DTO.Authorization.ErrorCode.AccountIsNotActivated"/></exception>
    /// <exception cref="ForbiddenException{T}"> <see cref="DTO.Authorization.ErrorCode.AccountPasswordDontMatch"/></exception>
    [HttpPost("/login")]
    public async Task<ActionResult<UserAccessTokenDTO>> SignIn(LoginUserDTO loginUserDto)
    {
        var token = _authorizationService.Authorize(loginUserDto.Username, loginUserDto.Password);
        return Ok(new UserAccessTokenDTO { Token = token.Value });
    }
}
