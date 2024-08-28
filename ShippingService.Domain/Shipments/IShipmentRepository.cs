using ShippingService.Domain.Shipments.Entities;

namespace ShippingService.Domain.Shipments;

public interface IShipmentRepository
{
    Task<bool> ExistsByOrderIdAsync(string orderId, CancellationToken cancellationToken);
    
    Task AddAsync(Shipment shipment, CancellationToken cancellationToken);
    
    Task<Shipment?> GetByNumberAsync(string shipmentNumber, CancellationToken cancellationToken);
}