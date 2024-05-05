using CDS.Contracts.Interfaces.Cache;

namespace CDS.Contracts.Models.Cache;

public class AssetFileUrlCache(string fileUrl, DateOnly distributionDate) : ICacheable {
    public string FileUrl { get; set; } = fileUrl;
    public DateOnly DistributionDate { get; set; } = distributionDate;
}