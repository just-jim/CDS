using CDS.Application.Common.Interfaces.Cache;
using CDS.Application.Common.Interfaces.Database;
using CDS.Application.Common.Models;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.Entities;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CDS.Application.ContentDistributions.Commands.CreateContentDistribution;

public class CreateContentDistributionCommandHandler(
    IContentDistributionRepository contentDistributionRepository,
    ICacheService cache,
    ILogger<CreateContentDistributionCommandHandler> logger
    ) : IRequestHandler<CreateContentDistributionCommand, ErrorOr<ContentDistribution>> {
    
    public async Task<ErrorOr<ContentDistribution>> Handle(CreateContentDistributionCommand request, CancellationToken ct) {
        
        var contentDistribution = ContentDistribution.Create(
            distributionDate: request.DistributionDate,
            distributionChannel: request.DistributionChannel,
            distributionMethod: request.DistributionMethod,
            assetContentDistributions: request.AssetContentDistributions.ConvertAll(assetContentDistribution => AssetContentDistribution.Create(
                AssetId.Create(assetContentDistribution.AssetId),
                assetContentDistribution.FileUrl
                )
            )
        );
        
        await contentDistributionRepository.AddAsync(contentDistribution);
        
        // Cache the content distribution fileUrl for each asset
        foreach (var asset in contentDistribution.AssetContentDistributions) {

            var currentCachedUrl = (AssetFileUrlCache?)cache.Get(asset.AssetId.Value, typeof(AssetFileUrlCache));
            
            if (currentCachedUrl.IsMissing() || currentCachedUrl.IsOutdated(contentDistribution.DistributionDate)) {
                logger.LogInformation($"Caching the fileUrl for the asset {asset.AssetId.Value}");
                cache.Set(asset.AssetId.Value, new AssetFileUrlCache(asset.FileUrl, contentDistribution.DistributionDate));
            } else {
                string formatedDistributionDate = currentCachedUrl!.DistributionDate.ToString("yyyy-MM-dd");
                logger.LogInformation($"A more recent version ({formatedDistributionDate}) of the content distribution url is already cached for the asset {asset.AssetId.Value}");
            }
        }
        
        return contentDistribution;
    }
}

public static class AssetFileUrlCacheExtensions
{
    public static bool IsMissing(this AssetFileUrlCache? cache) {
        return cache == null;
    }
    
    public static bool IsOutdated(this AssetFileUrlCache? cache, DateOnly comparedToDate) {
        return cache?.DistributionDate < comparedToDate;
    }
}