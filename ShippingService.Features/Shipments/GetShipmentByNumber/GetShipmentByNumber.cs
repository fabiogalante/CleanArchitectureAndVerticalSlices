using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingService.Domain.Shipments;
using ShippingService.Features.Abstract;
using ShippingService.Features.Shipments.CreateShipment;
using ShippingService.Features.Shipments.Shared.Responses;

namespace ShippingService.Features.Shipments.GetShipmentByNumber;

internal sealed record GetShipmentByNumberQuery(string ShipmentNumber)
	: IRequest<ShipmentResponse?>;

internal sealed class GetShipmentByNumberQueryHandler(
	IShipmentRepository repository,
	ILogger<GetShipmentByNumberQueryHandler> logger)
	: IRequestHandler<GetShipmentByNumberQuery, ShipmentResponse?>
{
	public async Task<ShipmentResponse?> Handle(GetShipmentByNumberQuery request, CancellationToken cancellationToken)
	{
		var shipment = await repository.GetByNumberAsync(request.ShipmentNumber, cancellationToken);
		if (shipment is null)
		{
			logger.LogDebug("Shipment with number {ShipmentNumber} not found", request.ShipmentNumber);
			return null;
		}

		var response = shipment.MapToResponse();
		return response;
	}
}

public class GetShipmentByNumberEndpoint : IEndpoint
{
	public void MapEndpoint(WebApplication app)
	{
		app.MapGet("/api/shipments/{shipmentNumber}", Handle);
	}
	
	private static async Task<IResult> Handle(
		[FromRoute] string shipmentNumber,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		var response = await mediator.Send(new GetShipmentByNumberQuery(shipmentNumber), cancellationToken);
		return response is not null ? Results.Ok(response) : Results.NotFound($"Shipment with number '{shipmentNumber}' not found");
	}
}
