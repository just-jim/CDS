namespace CDS.Infrastructure.SqsConsumers.ContentDistributionDomainConsumer.Models.Sqs;

public class ContentDistributionDomainAsset {
    public string AssetId { get; set; }
    public string Name { get; set; }
    public string FileURL { get; set; }
}