using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using SimbirGO.Contexts;
using SimbirGO.Services;
using SimbirGO.Services.Security;
using SimbirGO.Models;
using SimbirGO.DTO.User;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//Конфигурация подключения к БД
builder.Services.AddDbContext<SimbirDbContext>(op =>
    op.UseNpgsql(builder.Configuration.GetValue<string>("Databases:Connection")));

// Добавления авторизации
var jwtOptions = new JWTOptions();
builder.Configuration.GetSection("Security:JWT").Bind(jwtOptions);
SetupAuthorization(jwtOptions);

// Регистрация в DI генератора токенов
builder.Services.AddSingleton<ITokenGenerator>(_ => new JWTTokenGenerator(jwtOptions));
// Регистрация в DI фабрики генератора хэша с солью
builder.Services.AddSingleton<HashWithSaltGeneratorFactory>();

AddMapster(builder.Services);

builder.Services.AddScoped<IAuthService, JWTAuthService>();
builder.Services.AddControllers().AddNewtonsoftJson(); ;
builder.Services.AddEndpointsApiExplorer();

/* NOTE!
 * В сваггере в поле Authorize
 * надо вводить не просто токен, а в формате
 * " Bearer {token} " 
 */
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization: `Bearer JWT-token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors();

// Добавление root пользователя
var rootUserOptions = new RootUserOptions();
builder.Configuration.GetSection("RootUser").Bind(rootUserOptions);
await AddRootUser(rootUserOptions);

// Обход авторизации во время разработки
app.MapControllers();
//if (app.Environment.IsDevelopment())
//    app.MapControllers().AllowAnonymous();

app.Run();

// Добавление авторизации
void SetupAuthorization(JWTOptions options)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Default", new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());
    });
    // Конфигурация валидатора токенов
    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(bearerOptions =>
        {
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                //Валидация предоставителя токена
                ValidateIssuer = true,
                //Установка предоставителя токена
                ValidIssuer = options.Issuer,

                //Валидация получателя
                ValidateAudience = true,

                //Установка получателя
                ValidAudience = options.Audience,

                //Валидация по времени жизни токена
                ValidateLifetime = true,

                //Валидация по ключу безопасности
                ValidateIssuerSigningKey = true,

                //Установка ключа безопасности
                IssuerSigningKey = options.GetSecurityKey()
            };
        }
        );
}

// Добавление root пользователя, если он отсутствует в системе
async Task AddRootUser(RootUserOptions options)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetService<SimbirDbContext>();
    var service = scope.ServiceProvider.GetService<IAuthService>();

    if (context!.Users.Any(user => user.RoleOfUser == User.Roles.Root))
        return;

    var user = options.Adapt<User>();

    await service?.Registrate(user, User.Roles.Root, "root")!;

    logger?.Log(LogLevel.Information, "Root пользователь был добавлен");
}

// Добавление конфигурации mapster
void AddMapster(IServiceCollection serviceCollection)
{
    var config = new TypeAdapterConfig();

    config.NewConfig<UpdateUserDTO, User>().IgnoreNullValues(true);

    serviceCollection.AddSingleton(config);
    serviceCollection.AddScoped<IMapper, ServiceMapper>();
}
