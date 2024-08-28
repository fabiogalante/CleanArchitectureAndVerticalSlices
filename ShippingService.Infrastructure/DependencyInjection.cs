using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShippingService.Domain;
using ShippingService.Domain.Shipments;
using ShippingService.Infrastructure.Database;
using ShippingService.Infrastructure.Repositories;
using ShippingService.Infrastructure.Seeding;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("Postgres");

        services.AddDbContext<ShippingDbContext>(x => x
            .EnableSensitiveDataLogging()
            .UseNpgsql(postgresConnectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__MyMigrationsHistory", "shipping"))
            .UseSnakeCaseNamingConvention()
        );
        
        services.AddScoped<SeedService>();
        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<ShippingDbContext>());

        return services;
    }
}