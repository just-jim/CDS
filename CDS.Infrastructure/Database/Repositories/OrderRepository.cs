using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CDS.Infrastructure.Database.Repositories;

public class OrderRepository(CdsDbContext dbContext) : IOrderRepository {
    public async Task AddAsync(Order order) {
        dbContext.Add(order);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsAsync(OrderId orderId) {
        return await dbContext.Orders.AnyAsync(order => order.Id == orderId);
    }
    
    public async Task<List<Order>> FindOrdersByAssetId(AssetId assetId)
    {
        return await dbContext.Orders
            .Where(o => o.AssetOrders.Any(ao => ao.AssetId == assetId))
            .ToListAsync();
    }
    
    public async Task<Unit> ResetAsync() {
        dbContext.Orders.RemoveRange(dbContext.Orders);
        await dbContext.SaveChangesAsync();
        return Unit.Value;
    }
}