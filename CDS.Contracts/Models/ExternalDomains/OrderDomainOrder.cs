using CDS.Contracts.Interfaces.Consumers;

namespace CDS.Contracts.Models.ExternalDomains;

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