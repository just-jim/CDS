using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;
using MediatR;

namespace CDS.Contracts.Interfaces.Database;

public interface IAssetRepository {
    Task UpdateAsync(Asset asset);
    Task AddAsync(Asset asset);
    Task<Asset?> GetByIdAsync(AssetId assetId);
    Task<bool> ExistsAsync(AssetId assetId);
    Task<List<Asset>> ListAsync(int pageNumber, int pageSize);
    Task<Unit> ResetAsync();
}