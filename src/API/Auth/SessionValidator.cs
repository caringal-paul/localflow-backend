using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace API.Auth;

public static class SessionValidator
{
    public static async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        var stamp = context.Principal?.FindFirstValue(AppClaims.SecurityStamp);
        var valid = false;

        if (Guid.TryParse(context.Principal?.FindFirstValue(AppClaims.UserId), out var userId))
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();
            valid = await db.Users.AnyAsync(
                u => u.Id == userId && u.IsActive && u.SecurityStamp == stamp,
                context.HttpContext.RequestAborted);
        }

        if (!valid)
        {
            context.RejectPrincipal();
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}