using Microsoft.AspNetCore.Builder;

namespace ShippingService.Features.Abstract;

public interface IEndpoint
{
    void MapEndpoint(WebApplication app);
}