using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.GetAsset;

public class GetAssetQueryHandler(IAssetRepository assetRepository) : IRequestHandler<GetAssetQuery, ErrorOr<Asset>> {

    public async Task<ErrorOr<Asset>> Handle(GetAssetQuery query, CancellationToken ct) {
        var assetId = AssetId.Create(query.AssetId);
        
        if (!await assetRepository.ExistsAsync(assetId)) {
            return Errors.AssetError.NotFound;
        }
        
        return (await assetRepository.GetByIdAsync(assetId))!;
    }
}