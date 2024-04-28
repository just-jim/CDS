﻿using CDS.Adapters.Interfaces;

namespace CDS.Adapters.OrderDomainAdapter.Models.Sqs;

public class OrderDomainOrder : IMessage {
    public string OrderNumber { get; set; }
    public string CustomerName { get; set; }
    public string OrderDate { get; set; }
    public int TotalAssets { get; set; }
    public List<OrderDomainAsset> Assets { get; set; }

    string IMessage.Id() {
        return OrderNumber;
    }
}