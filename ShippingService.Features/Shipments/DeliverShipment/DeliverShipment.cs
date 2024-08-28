using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingService.Domain;
using ShippingService.Domain.Shipments;
using ShippingService.Features.Abstract;
using ShippingService.Features.Extensions;

namespace ShippingService.Features.Shipments.DeliverShipment;

public class DeliverShipmentEndpoint : IEndpoint
{
	public void MapEndpoint(WebApplication app)
	{
		app.MapPost("/api/shipments/deliver/{shipmentNumber}", Handle);
	}

	private static async Task<IResult> Handle(
		[FromRoute] string shipmentNumber,
		IShipmentRepository repository,
		IUnitOfWork unitOfWork,
		ILogger<DeliverShipmentEndpoint> logger,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		var shipment = await repository.GetByNumberAsync(shipmentNumber, cancellationToken);
		if (shipment is null)
		{
			logger.LogDebug("Shipment with number {ShipmentNumber} not found", shipmentNumber);
			return Error.NotFound("Shipment.NotFound", $"Shipment with number '{shipmentNumber}' not found").ToProblem();
		}

		var response = shipment.Deliver();
		if (response.IsError)
		{
			return response.Errors.ToProblem();
		}
		
		await unitOfWork.SaveChangesAsync(cancellationToken);

		logger.LogInformation("Delivered shipment with {ShipmentNumber}", shipmentNumber);
		return Results.NoContent();
	}
}
