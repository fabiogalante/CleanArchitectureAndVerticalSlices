using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using ShippingService.Features.Shipments.CreateShipment;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterEndpointsFromAssemblyContaining<CreateShipmentEndpoint>();
        
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<CreateShipmentEndpoint>();
        });
        
        services.AddValidatorsFromAssemblyContaining<CreateShipmentEndpoint>();
        
        services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    public static WebApplication MapFeatures(this WebApplication app)
    {
        app.MapEndpoints();
        return app;
    } 
}