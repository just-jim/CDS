using CDS.Contracts.Interfaces.Cache;
using CDS.Contracts.Interfaces.Database;
using CDS.Contracts.Models.Cache;
using CDS.Contracts.Queries;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CDS.Application.Assets.Queries.GetAsset;

public class GetAssetQueryHandler(
    IAssetRepository assetRepository,
    IContentDistributionRepository contentDistributionRepository,
    ICacheService cache,
    ILogger<GetAssetQueryHandler> logger
) : IRequestHandler<GetAssetQuery, ErrorOr<string>> {

    public async Task<ErrorOr<string>> Handle(GetAssetQuery query, CancellationToken ct) {
        var assetId = AssetId.Create(query.AssetId);

        if (!await assetRepository.ExistsAsync(assetId)) {
            return Errors.AssetError.NotFound;
        }

        var currentCachedUrl = (AssetFileUrlCache?)cache.Get(assetId.Value, typeof(AssetFileUrlCache));

        if (currentCachedUrl != null) {
            logger.LogInformation($"Fetched the file url for the asset '{assetId.Value}' from the cache");
            return currentCachedUrl.FileUrl;
        }

        // Get the most recent contentDistribution
        var contentDistribution = await contentDistributionRepository.GetMostRecentContentDistributionForAnAssetIdAsync(assetId);
        if (contentDistribution?.AssetContentDistributions == null) {
            logger.LogInformation($"Couldn't find a content distribution file url for the asset '{assetId.Value}'");
            return Errors.AssetError.FileUrlNotFound;
        }
        var assetContentDistribution = contentDistribution.AssetContentDistributions.First(acd => acd.AssetId == assetId);
        string formatedDate = contentDistribution.DistributionDate.ToString("yyyy-MM-dd");
        logger.LogInformation($"Fetched the most recent file url for the asset '{assetId.Value}' from the db. Date: {formatedDate}");

        // Cache it
        logger.LogInformation($"Caching the file url for the asset '{assetId.Value}'");
        cache.Set(
            assetContentDistribution.AssetId.Value,
            new AssetFileUrlCache(assetContentDistribution.FileUrl, contentDistribution.DistributionDate),
            typeof(AssetFileUrlCache)
        );

        // Return it
        return assetContentDistribution.FileUrl;
    }
}