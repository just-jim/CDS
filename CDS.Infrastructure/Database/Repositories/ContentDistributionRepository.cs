using CDS.Contracts.Interfaces.Database;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.ContentDistributionAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CDS.Infrastructure.Database.Repositories;

public class ContentDistributionRepository(CdsDbContext dbContext) : IContentDistributionRepository {
    public async Task AddAsync(ContentDistribution contentDistribution) {
        dbContext.Add(contentDistribution);
        await dbContext.SaveChangesAsync();
    }

    public async Task<ContentDistribution?> GetMostRecentContentDistributionForAnAssetIdAsync(AssetId assetId) {
        return await dbContext.ContentDistributions
            .Where(cd => cd.AssetContentDistributions.Any(acd => acd.AssetId == assetId))
            .OrderByDescending(cd => cd.DistributionDate)
            .FirstOrDefaultAsync();
    }

    public async Task<List<ContentDistribution>> FindContentDistributionsByAssetId(AssetId assetId) {
        return await dbContext.ContentDistributions
            .Where(cd => cd.AssetContentDistributions.Any(acd => acd.AssetId == assetId))
            .ToListAsync();
    }

    public async Task<Unit> ResetAsync() {
        dbContext.ContentDistributions.RemoveRange(dbContext.ContentDistributions);
        await dbContext.SaveChangesAsync();
        return Unit.Value;
    }
}