namespace CDS.Infrastructure.SqsConsumers.OrderDomainConsumer.Models.Sqs;

public class OrderDomainAsset {
    public string AssetId { get; set; }
    public int Quantity { get; set; }
}