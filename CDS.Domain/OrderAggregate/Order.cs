using CDS.Domain.Common.Models;
using CDS.Domain.OrderAggregate.Entities;
using CDS.Domain.OrderAggregate.Events;
using CDS.Domain.OrderAggregate.ValueObjects;

namespace CDS.Domain.OrderAggregate;

public class Order : AggregateRoot<OrderId, string> {
    readonly List<AssetOrder> _assetOrders = [];
    
    public string OrderNumber { get; private set; }
    public string CustomerName { get; private set; }
    public DateOnly OrderDate { get; private set; }
    public int TotalAssets { get; private set; }
    public IReadOnlyList<AssetOrder> AssetOrders => _assetOrders.AsReadOnly();

    Order(
        string orderNumber,
        string customerName,
        DateOnly orderDate,
        int totalAssets,
        List<AssetOrder> assetOrders
    ) : base(OrderId.Create(orderNumber)) {
        OrderNumber = orderNumber;
        CustomerName = customerName;
        OrderDate = orderDate;
        TotalAssets = totalAssets;
        _assetOrders = assetOrders;
    }

    public static Order Create(
        string orderNumber, 
        string customerName, 
        DateOnly orderDate, 
        int totalAssets,
        List<AssetOrder> assetOrders
    ) {
        var order = new Order(orderNumber, customerName, orderDate, totalAssets, assetOrders);
        order.AddDomainEvent(new OrderCreated(order));
        return order;
    }
    
#pragma warning disable CS8618
    Order() { }
#pragma warning restore CS8618
}