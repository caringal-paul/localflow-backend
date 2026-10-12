using System.Security.Claims;
using Application.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace API.Auth;

public static class SessionValidator
{
    public static async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        var stamp = context.Principal?.FindFirstValue(AppClaims.SecurityStamp);
        var valid = false;

        if (Guid.TryParse(context.Principal?.FindFirstValue(AppClaims.UserId), out var userId))
        {
            var users = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            valid = await users.IsSessionValidAsync(userId, stamp, context.HttpContext.RequestAborted);
        }

        if (!valid)
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}