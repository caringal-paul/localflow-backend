using System.Security.Claims;
using API.Auth;
using Application.Common.Interfaces;

namespace API.Services;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public Guid? UserId => Read(AppClaims.UserId);
    public Guid? TenantId => Read(AppClaims.TenantId);
    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    private Guid? Read(string claim)
        => Guid.TryParse(Principal?.FindFirstValue(claim), out var id) ? id : null;
}