using Application.Common.Enums;
using Application.Common.Errors;

namespace Application.Auth;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = new("auth.invalid_credentials", "Invalid email or password.", ErrorType.Unauthorized);
    public static readonly Error AccountDisabled = new("auth.account_disabled", "This account is disabled.", ErrorType.Forbidden);
    public static readonly Error NoTenantAccess = new("auth.no_tenant_access", "You do not have access to any active company.", ErrorType.Forbidden);
}