using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;

namespace CDS.Application.Common.Interfaces.Database;

public interface IAssetRepository {
    Task UpdateAsync(Asset asset);
    Task AddAsync(Asset asset);
    Task<Asset?> GetByIdAsync(AssetId assetId);
    Task<bool> ExistsAsync(AssetId assetId);
}