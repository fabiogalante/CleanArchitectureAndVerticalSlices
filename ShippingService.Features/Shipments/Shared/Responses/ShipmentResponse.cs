using ShippingService.Domain.Shipments.Entities;

namespace ShippingService.Features.Shipments.Shared.Responses;

public sealed record ShipmentResponse(
    string Number,
    string OrderId,
    Address Address,
    string Carrier,
    string ReceiverEmail,
    ShipmentStatus Status,
    List<ShipmentItemResponse> Items);