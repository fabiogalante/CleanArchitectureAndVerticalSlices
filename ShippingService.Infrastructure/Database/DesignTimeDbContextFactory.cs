using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShippingService.Infrastructure.Database;

/// <summary>
/// A db context factory used tools for EF Core to create migrations for <see cref="ShippingDbContext"/>
/// </summary>
public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<ShippingDbContext>
{
    public ShippingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShippingDbContext>()
            .UseNpgsql(npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__MyMigrationsHistory", "shipping"))
            .UseSnakeCaseNamingConvention();

        return new ShippingDbContext(optionsBuilder.Options);
    }
}