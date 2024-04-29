using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.ValueObjects;

namespace CDS.Application.Common.Interfaces.Database;

public interface IOrderRepository {
    Task AddAsync(Order order);
    Task<bool> ExistsAsync(OrderId orderId);
}