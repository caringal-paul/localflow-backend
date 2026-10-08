using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Feature services
        // services.AddScoped<IOrderService, OrderService>();
        // services.AddScoped<IUserService, UserService>();

        return services;
    }
}