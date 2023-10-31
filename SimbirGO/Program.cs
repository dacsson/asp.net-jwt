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

//������������ ����������� � ��
builder.Services.AddDbContext<SimbirDbContext>(op =>
    op.UseNpgsql(builder.Configuration.GetValue<string>("Databases:Connection")));

// ���������� �����������
var jwtOptions = new JWTOptions();
builder.Configuration.GetSection("Security:JWT").Bind(jwtOptions);
SetupAuthorization(jwtOptions);

// ����������� � DI ���������� �������
builder.Services.AddSingleton<ITokenGenerator>(_ => new JWTTokenGenerator(jwtOptions));
// ����������� � DI ������� ���������� ���� � �����
builder.Services.AddSingleton<HashWithSaltGeneratorFactory>();

AddMapster(builder.Services);

builder.Services.AddScoped<IAuthService, JWTAuthService>();
builder.Services.AddControllers().AddNewtonsoftJson(); ;
builder.Services.AddEndpointsApiExplorer();

/* NOTE!
 * � �������� � ���� Authorize
 * ���� ������� �� ������ �����, � � �������
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

// ���������� root ������������
var rootUserOptions = new RootUserOptions();
builder.Configuration.GetSection("RootUser").Bind(rootUserOptions);
await AddRootUser(rootUserOptions);

// ����� ����������� �� ����� ����������
app.MapControllers();
//if (app.Environment.IsDevelopment())
//    app.MapControllers().AllowAnonymous();

app.Run();

// ���������� �����������
void SetupAuthorization(JWTOptions options)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Default", new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());
    });
    // ������������ ���������� �������
    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(bearerOptions =>
        {
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                //��������� �������������� ������
                ValidateIssuer = true,
                //��������� �������������� ������
                ValidIssuer = options.Issuer,

                //��������� ����������
                ValidateAudience = true,

                //��������� ����������
                ValidAudience = options.Audience,

                //��������� �� ������� ����� ������
                ValidateLifetime = true,

                //��������� �� ����� ������������
                ValidateIssuerSigningKey = true,

                //��������� ����� ������������
                IssuerSigningKey = options.GetSecurityKey()
            };
        }
        );
}

// ���������� root ������������, ���� �� ����������� � �������
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

    logger?.Log(LogLevel.Information, "Root ������������ ��� ��������");
}

// ���������� ������������ mapster
void AddMapster(IServiceCollection serviceCollection)
{
    var config = new TypeAdapterConfig();

    config.NewConfig<UpdateUserDTO, User>().IgnoreNullValues(true);

    serviceCollection.AddSingleton(config);
    serviceCollection.AddScoped<IMapper, ServiceMapper>();
}
