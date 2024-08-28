namespace ShippingService.Domain.Shipments.Entities;

public enum ShipmentStatus
{
	Created,
	Processing,
	Dispatched,
	InTransit,
	Delivered,
	Received,
	Cancelled
}
