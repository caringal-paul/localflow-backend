using API.Auth;
using API.Common;
using API.Filters;
using API.Middleware;
using API.Services;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using API.Logging;
using Serilog.Core;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var sameSite = configuration.GetValue("Auth:CookieSameSite", SameSiteMode.Lax);
        var cookieDomain = configuration["Auth:CookieDomain"];

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3005",
                        "http://172.236.143.82:3001",
                        "https://altostratus.ami.opsolutions.biz")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddProblemDetails(); // required by UseExceptionHandler()
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.ConfigureHttpJsonOptions(o => ApiJson.Configure(o.SerializerOptions));

        services.AddControllers(o => o.Filters.Add<RequestValidationFilter>())
            .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
            .AddJsonOptions(o => ApiJson.Configure(o.JsonSerializerOptions));

        services.AddHttpContextAccessor();
        services.AddHttpContextAccessor();
        services.AddSingleton<ILogEventEnricher, CurrentUserEnricher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                o.Cookie.Name = "localflow.auth";
                o.Cookie.HttpOnly = true;
                o.Cookie.SameSite = sameSite;
                o.Cookie.Domain = cookieDomain;
                // SameSite=None requires Secure; otherwise follow the environment.
                o.Cookie.SecurePolicy = sameSite == SameSiteMode.None || !environment.IsDevelopment()
                    ? CookieSecurePolicy.Always
                    : CookieSecurePolicy.SameAsRequest;

                o.ExpireTimeSpan = TimeSpan.FromHours(8);
                o.SlidingExpiration = true;

                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SessionValidator.ValidateAsync,
                    // Status codes instead of redirects; StatusCodeResponseWriter adds the envelope.
                    OnRedirectToLogin = ctx => { ctx.Response.StatusCode = StatusCodes.Status401Unauthorized; return Task.CompletedTask; },
                    OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = StatusCodes.Status403Forbidden; return Task.CompletedTask; }
                };
            });

        // Secure by default: every endpoint requires login unless marked [AllowAnonymous].
        services.AddAuthorization(o => o.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();    

        return services;
    }
}