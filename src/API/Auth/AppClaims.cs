using System.Security.Claims;

namespace API.Auth;

public static class AppClaims
{
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string TenantId = "tenant_id";
    public const string SecurityStamp = "security_stamp";
}