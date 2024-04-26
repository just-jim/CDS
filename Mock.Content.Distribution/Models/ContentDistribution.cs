namespace Mock.Content.Distribution.Models;

public class ContentDistribution {
    public string DistributionDate { get; set; }
    public string DistributionChannel { get; set; }
    public string DistributionMethod { get; set; }
    public List<Asset> Assets { get; set; }
}