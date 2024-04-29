using CDS.Domain.Common.Models;
using CDS.Domain.OrderAggregate.Events;
using CDS.Domain.OrderAggregate.ValueObjects;

namespace CDS.Domain.OrderAggregate;

public class Order : AggregateRoot<OrderId, string> {
    public string OrderNumber { get; private set; }
    public string CustomerName { get; private set; }
    public DateOnly OrderDate { get; private set; }
    public int TotalAssets { get; private set; }

    Order(
        string orderNumber,
        string customerName,
        DateOnly orderDate,
        int totalAssets
    ) : base(OrderId.Create(orderNumber)) {
        OrderNumber = orderNumber;
        CustomerName = customerName;
        OrderDate = orderDate;
        TotalAssets = totalAssets;
    }

    public static Order Create(
        string orderNumber, 
        string customerName, 
        DateOnly orderDate, 
        int totalAssets
    ) {
        var order = new Order(orderNumber, customerName, orderDate, totalAssets);
        order.AddDomainEvent(new OrderCreated(order));
        return order;
    }
}