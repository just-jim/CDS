using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.Entities;

namespace CDS.Application.Common.Interfaces.Database;

public interface IContentDistributionRepository {
    Task AddAsync(ContentDistribution contentDistribution);
    Task<ContentDistribution?> GetMostRecentContentDistributionForAnAssetIdAsync(AssetId assetId);
    Task<List<ContentDistribution>> FindContentDistributionsByAssetId(AssetId assetId);
}