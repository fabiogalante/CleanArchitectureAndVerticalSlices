using ShippingService.Domain.Shipments.Entities;
using ShippingService.Features.Shipments.Shared.Responses;

namespace ShippingService.Features.Shipments.CreateShipment;

internal static class CreateShipmentMappingExtensions
{
    public static CreateShipmentCommand MapToCommand(this CreateShipmentRequest request)
        => new(request.OrderId,
            request.Address,
            request.Carrier,
            request.ReceiverEmail,
            request.Items);
    
    public static Shipment MapToShipment(this CreateShipmentCommand command, string shipmentNumber)
        => Shipment.Create
        (
            shipmentNumber,
            command.OrderId,
            command.Address,
            command.Carrier,
            command.ReceiverEmail,
            command.Items
        );

    public static ShipmentResponse MapToResponse(this Shipment shipment)
        => new(
            shipment.Number,
            shipment.OrderId,
            shipment.Address,
            shipment.Carrier,
            shipment.ReceiverEmail,
            shipment.Status,
            shipment.Items
                .Select(x => new ShipmentItemResponse(x.Product, x.Quantity))
                .ToList()
            );
}