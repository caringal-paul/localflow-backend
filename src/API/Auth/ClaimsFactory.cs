using System.Security.Claims;
using Application.Auth.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace API.Auth;

public static class ClaimsFactory
{
    public static ClaimsPrincipal Create(LoginResult user)
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(AppClaims.UserId, user.UserId.ToString()),
            new Claim(AppClaims.TenantId, user.TenantId.ToString()),
            new Claim(AppClaims.SecurityStamp, user.SecurityStamp),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName)
        ], CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }
}