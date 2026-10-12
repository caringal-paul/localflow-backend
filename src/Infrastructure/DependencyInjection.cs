using Application.Common.Interfaces;
using Application.TenantMembers;
using Application.Users;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantMemberRepository, TenantMemberRepository>();
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    o => o.UseNetTopologySuite())
                .UseSnakeCaseNamingConvention());

            // Repositories (only if we adopt them)
            // services.AddScoped<IOrderRepository, OrderRepository>();
            // services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}