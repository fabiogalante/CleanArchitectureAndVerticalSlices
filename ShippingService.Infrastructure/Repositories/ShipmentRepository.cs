using Microsoft.EntityFrameworkCore;
using ShippingService.Domain.Shipments;
using ShippingService.Domain.Shipments.Entities;
using ShippingService.Infrastructure.Database;

namespace ShippingService.Infrastructure.Repositories;

public class ShipmentRepository(ShippingDbContext dbContext) : IShipmentRepository
{
    public async Task<bool> ExistsByOrderIdAsync(string orderId, CancellationToken cancellationToken)
    {
        return await dbContext.Shipments
            .Where(s => s.OrderId == orderId)
            .AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Shipment shipment, CancellationToken cancellationToken)
    {
        await dbContext.Shipments.AddAsync(shipment, cancellationToken);
    }

    public async Task<Shipment?> GetByNumberAsync(string shipmentNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Shipments
            .Include(x => x.Items)
            .Where(s => s.Number == shipmentNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }
}