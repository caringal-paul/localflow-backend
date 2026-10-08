using Application.Common.Interfaces;
using API.Services;
using API.Middleware;
using API.Common;
using API.Filters;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3005")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.ConfigureHttpJsonOptions(o => ApiJson.Configure(o.SerializerOptions));

        services.AddControllers(o => o.Filters.Add<RequestValidationFilter>())
            .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
            .AddJsonOptions(o => ApiJson.Configure(o.JsonSerializerOptions));

        return services;
    }
}