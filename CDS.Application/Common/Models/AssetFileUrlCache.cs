using CDS.Application.Common.Interfaces.Cache;

namespace CDS.Application.Common.Models;

public class AssetFileUrlCache(string fileUrl, DateOnly distributionDate) : ICacheable {
    public string FileUrl { get; set; } = fileUrl;
    public DateOnly DistributionDate { get; set; } = distributionDate;
}