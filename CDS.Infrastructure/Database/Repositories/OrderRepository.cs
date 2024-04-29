using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.ValueObjects;
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
}