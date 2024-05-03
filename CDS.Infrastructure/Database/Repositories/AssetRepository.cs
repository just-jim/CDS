using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CDS.Infrastructure.Database.Repositories;

public class AssetRepository(CdsDbContext dbContext) : IAssetRepository {

    public async Task AddAsync(Asset asset) {
        dbContext.Add(asset);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(AssetId assetId) {
        return await dbContext.Assets.AnyAsync(asset => asset.Id == assetId);
    }

    public async Task<Asset?> GetByIdAsync(AssetId assetId) {
        return await dbContext.Assets.FirstOrDefaultAsync(asset => asset.Id == assetId);
    }

    public async Task UpdateAsync(Asset asset) {
        dbContext.Assets.Update(asset);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Asset>> ListAsync() {
        return await dbContext.Assets.ToListAsync();
    }
}