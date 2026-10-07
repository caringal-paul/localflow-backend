using System.Security.Claims;
using Application.Common.Interfaces;

namespace API.Services;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public Guid? UserId => Guid.TryParse(Principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
    public Guid? TenantId => Guid.TryParse(Principal?.FindFirstValue("tenant_id"), out var id) ? id : null;
}