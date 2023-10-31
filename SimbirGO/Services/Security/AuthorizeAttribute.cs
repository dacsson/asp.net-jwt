using Microsoft.AspNetCore.Authorization;
using SimbirGO.Models;
using System.Text;

namespace SimbirGO.Services.Security;

/// <summary>
/// Авторизация через enum
/// </summary>
public class AuthorizeUserAttribute : AuthorizeAttribute
{
    public AuthorizeUserAttribute(params User.Roles[] rolesArray)
    {
        var builder = new StringBuilder();
        foreach (var role in rolesArray)
        {
            builder.Append(role.ToString());
            builder.Append(',');
        }

        Console.WriteLine();

        Roles = builder.ToString().TrimEnd(',');
    }
}
