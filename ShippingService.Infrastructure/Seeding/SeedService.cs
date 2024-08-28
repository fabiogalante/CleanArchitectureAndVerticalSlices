using Bogus;
using Microsoft.EntityFrameworkCore;
using ShippingService.Domain.Shipments.Entities;
using ShippingService.Infrastructure.Database;

namespace ShippingService.Infrastructure.Seeding;

public class SeedService(ShippingDbContext context)
{
	public async Task SeedDataAsync()
	{
		if (await context.Shipments.CountAsync(_ => true) > 0)
		{
			return;
		}

		var fakeShipments = new Faker<Shipment>()
			.CustomInstantiator(f => Shipment.Create(
				f.Commerce.Ean8(),
				f.Commerce.Ean13(),
				new Address
				{
					Street = f.Address.StreetAddress(),
					City = f.Address.City(),
					Zip = f.Address.ZipCode()
				},
				f.Commerce.Department(),
				"TODO: SET EMAIL HERE",
				Enumerable.Range(1, f.Random.Int(1, 10))
					.Select(_ => new ShipmentItem
					{
						Product = f.Commerce.Ean8(),
						Quantity = f.Random.Int(1, 5)
					})
					.ToList()
			));

		var shipments = fakeShipments.Generate(10);

		context.Shipments.AddRange(shipments);
		await context.SaveChangesAsync();
	}
}
