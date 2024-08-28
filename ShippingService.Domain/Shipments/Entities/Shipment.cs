using ErrorOr;

namespace ShippingService.Domain.Shipments.Entities;

public class Shipment
{
	private readonly List<ShipmentItem> _items = [];
	
	public Guid Id { get; private set; }
	
	public string Number { get; private set; }
	
	public string OrderId { get; private set; }
	
	public Address Address { get; private set; }
	
	public string Carrier { get; private set; }
	
	public string ReceiverEmail { get; private set; }
	
	public ShipmentStatus Status { get; private set; }
	
	public IReadOnlyList<ShipmentItem> Items => _items.AsReadOnly();
	
	public DateTime CreatedAt { get; private set; }
	
	public DateTime? UpdatedAt { get; private set; }
	
	private Shipment()
	{
	}

	public static Shipment Create(
		string number,
		string orderId,
		Address address,
		string carrier,
		string receiverEmail,
		List<ShipmentItem> items)
	{
		var shipment = new Shipment
		{
			Id = Guid.NewGuid(),
			Number = number,
			OrderId = orderId,
			Address = address,
			Carrier = carrier,
			ReceiverEmail = receiverEmail,
			Status = ShipmentStatus.Created,
			CreatedAt = DateTime.UtcNow
		};
		
		shipment.AddItems(items);

		return shipment;
	}
	
	public void AddItem(ShipmentItem item)
	{
		_items.Add(item);
		UpdatedAt = DateTime.UtcNow;
	}
	
	public void AddItems(List<ShipmentItem> items)
	{
		_items.AddRange(items);
		UpdatedAt = DateTime.UtcNow;
	}

	public void RemoveItem(ShipmentItem item)
	{
		_items.Remove(item);
		UpdatedAt = DateTime.UtcNow;
	}
	
	public void UpdateAddress(Address newAddress)
	{
		Address = newAddress;
		UpdatedAt = DateTime.UtcNow;
	}
	
	public ErrorOr<Success> Process()
	{
		if (Status is not ShipmentStatus.Created)
		{
			return Error.Validation("Can only update to Processing from Created status");
		}
		
		Status = ShipmentStatus.Processing;
		UpdatedAt = DateTime.UtcNow;

		return Result.Success;
	}

	public ErrorOr<Success> Dispatch()
	{
		if (Status is not ShipmentStatus.Processing)
		{
			return Error.Validation("Can only update to Dispatched from Processing status");
		}
		
		Status = ShipmentStatus.Dispatched;
		UpdatedAt = DateTime.UtcNow;
		
		return Result.Success;
	}

	public ErrorOr<Success> Transit()
	{
		if (Status is not ShipmentStatus.Dispatched)
		{
			return Error.Validation("Can only update to InTransit from Dispatched status");
		}
		
		Status = ShipmentStatus.InTransit;
		UpdatedAt = DateTime.UtcNow;
		
		return Result.Success;
	}

	public ErrorOr<Success> Deliver()
	{
		if (Status is not ShipmentStatus.InTransit)
		{
			return Error.Validation("Can only update to Delivered from InTransit status");
		}
		
		Status = ShipmentStatus.Delivered;
		UpdatedAt = DateTime.UtcNow;
		
		return Result.Success;
	}
	
	public ErrorOr<Success> Receive()
	{
		if (Status is not ShipmentStatus.Delivered)
		{
			return Error.Validation("Can only update to Received from Delivered status");
		}
		
		Status = ShipmentStatus.Received;
		UpdatedAt = DateTime.UtcNow;
		
		return Result.Success;
	}

	public ErrorOr<Success> Cancel()
	{
		if (Status is ShipmentStatus.Delivered)
		{
			return Error.Validation("Cannot cancel a Delivered shipment");
		}
		
		Status = ShipmentStatus.Cancelled;
		UpdatedAt = DateTime.UtcNow;
		
		return Result.Success;
	}
}
