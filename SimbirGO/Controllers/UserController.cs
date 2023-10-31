using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimbirGO.Contexts;
using SimbirGO.DTO.Auth;
using SimbirGO.DTO.User;
using SimbirGO.Models;
using SimbirGO.Services;
using SimbirGO.Services.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimbirGO.Controllers;

/// <summary>
/// Контроллер для управления пользователями системы
/// </summary>
[Route("api/users")]
[ApiController]
public class UserController : Controller
{
    private readonly SimbirDbContext _dbContext;
    private readonly IAuthService _authorizationService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Создание экземпляра класса <see cref="UserController"/>
    /// </summary>
    /// <param name="authorizationService">Экземляр сервиса авторизации</param>
    /// <param name="optilogsDbContext">Экземпляр контекста базы данных Optilogs</param>
    /// <param name="mapper">Экземпляр маппера</param>
    public UserController(SimbirDbContext dbContext, IAuthService authorizationService, IMapper mapper)
    {
        _dbContext = dbContext;
        _authorizationService = authorizationService;
        _mapper = mapper;
    }

    /// <summary>
    /// Возвращает всех пользователей
    /// </summary>
    /// <returns>Список информации о пользователях</returns>
    [HttpGet]
    [Authorize(Roles = "Root")]
    public async Task<ActionResult<IList<LoginUserDTO>>> GetUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return Ok(users.Adapt<IList<LoginUserDTO>>());
    }

    /// <summary>
    /// Создает нового пользователя в системе
    /// </summary>
    /// <returns>Результат создания пользователя</returns>
    [HttpPost]
    [Authorize(Roles = "Root")]
    public async Task<ActionResult> CreateUser(UserDTO addUserDto)
    {
        var profileToAdd = addUserDto.Adapt<User>();
        await _authorizationService.Registrate(profileToAdd, addUserDto.RoleOfUser, addUserDto.Password);
        return Ok();
    }
}
