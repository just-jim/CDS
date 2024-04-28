using CDS.Adapters.Interfaces;

namespace CDS.Adapters.ContentDistributionDomainAdapter.Models.Sqs;

public class ContentDistributionDomainContentDistribution : IMessage {
    public string DistributionDate { get; set; }
    public string DistributionChannel { get; set; }
    public string DistributionMethod { get; set; }
    public List<ContentDistributionDomainAsset> Assets { get; set; }

    string IMessage.Id() {
        return DistributionDate;
    }
}