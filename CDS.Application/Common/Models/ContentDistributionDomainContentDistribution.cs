using CDS.Application.Common.Interfaces.Consumers;

namespace CDS.Application.Common.Models;

public class ContentDistributionDomainContentDistribution : IMessage {
    public string DistributionDate { get; set; }
    public string DistributionChannel { get; set; }
    public string DistributionMethod { get; set; }
    public List<ContentDistributionDomainAsset> Assets { get; set; }

    string IMessage.Id() {
        return DistributionDate;
    }
}