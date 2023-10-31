using SimbirGO.Contexts;
using SimbirGO.Models;
using SimbirGO.Pages;
using SimbirGO.Services.Security;
using SimbirGO.Exceptions;
using System.Net.Sockets;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace SimbirGO.Services;

public class JWTAuthService : IAuthService
{
    /// <summary>
    /// База данных
    /// </summary>
    private readonly SimbirDbContext _dbContext;

    /// <summary>
    /// Генератор токенов
    /// </summary>
    private readonly ITokenGenerator _tokenGenerator;

    /// <summary>
    /// Фабрика генераторов хэша паролей
    /// </summary>
    private readonly HashWithSaltGeneratorFactory _hashGeneratorfactory;

    public JWTAuthService(ITokenGenerator tokenGenerator, 
        HashWithSaltGeneratorFactory hashGeneratorfactory,
        SimbirDbContext simbirDbContext)
    {
        _tokenGenerator = tokenGenerator;
        _hashGeneratorfactory = hashGeneratorfactory;
        _dbContext= simbirDbContext;
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="username">Логин пользователя</param>
    /// <param name="password">пароль пользователя</param>
    /// <returns>JWT Токен</returns>
    /// <exception cref="NotFoundException{T}"><see cref="ErrorCode.AccountNotFound"/></exception>
    /// <exception cref="ForbiddenException{T}"> <see cref="ErrorCode.AccountIsNotActivated"/></exception>
    /// <exception cref="ForbiddenException{T}"> <see cref="ErrorCode.AccountPasswordDontMatch"/></exception>
    public Token Authorize(string username, string password)
    {
        var userToLogIn = new User { Username = username };

        var userInDb = _dbContext.Users
            .FirstOrDefault(account => account.Username == userToLogIn.Username) ??
            throw new NotFoundException<ErrorCode>(ErrorCode.AccountNotFound);

        var hash = _hashGeneratorfactory.Create(new byte[]{ 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }).GenerateHash(password);
        if (userInDb.PasswordHash != null && !userInDb.PasswordHash.SequenceEqual(hash))
            throw new ForbiddenException<ErrorCode>(ErrorCode.AccountPasswordDontMatch);

        var generatedToken = _tokenGenerator.GenerateToken(userInDb);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(generatedToken);
        return new Token(tokenString);
    }

    /// <summary>
    /// Регистрация пользователя в системе
    /// </summary>
    /// <param name="user">данные профиля</param>
    /// <param name="role">Роль пользователя</param>
    /// <returns>JWT Токен</returns>
    /// <exception cref="NotFoundException{T}"><see cref="ErrorCode.AccountAlreadyExist"/></exception>
    public async Task Registrate(User user, User.Roles role, string password)
    {
        if (await _dbContext.Users.AnyAsync(account => account.Username == user.Username))
            throw new NotFoundException<ErrorCode>(ErrorCode.AccountAlreadyExist);

        var _user = new User { RoleOfUser = role, Username = user.Username };
        _user.PasswordHash = GenerateHashes(password);
        _dbContext.Users.Add(_user);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Генерирует хэши пароля и соли
    /// </summary>
    /// <param name="password">пароль пользователя</param>
    /// <returns>Кортеж из хэша пароля и хеша соли</returns>
    public byte[] GenerateHashes(string password)
    {
        var passwordHash = _hashGeneratorfactory.Create(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }).GenerateHash(password);

        return passwordHash;
    }
}
