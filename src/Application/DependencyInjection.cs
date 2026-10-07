using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Feature services go here, e.g. services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}