using Serilog.Core;
using Serilog.Events;
using API.Auth;

namespace API.Logging;

public sealed class CurrentUserEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true) return;

        AddIfPresent(logEvent, propertyFactory, "UserId", user.FindFirst(AppClaims.UserId)?.Value);
        AddIfPresent(logEvent, propertyFactory, "TenantId", user.FindFirst(AppClaims.TenantId)?.Value);
    }

    private static void AddIfPresent(LogEvent logEvent, ILogEventPropertyFactory factory, string name, string? value)
    {
        if (value is null) return;
        logEvent.AddPropertyIfAbsent(factory.CreateProperty(name, value));
    }
}