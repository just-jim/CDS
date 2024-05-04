using CDS.Application.Common.Interfaces.Database;
using CDS.Contracts;
using CDS.Domain.AssetAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.ListAssets;

public class ListAssetsQueryHandler(IAssetRepository assetRepository) : IRequestHandler<ListAssetsQuery, ErrorOr<List<AssetShortResponse>>> {

    public async Task<ErrorOr<List<AssetShortResponse>>> Handle(ListAssetsQuery query, CancellationToken ct) {
        List<Asset> assets = await assetRepository.ListAsync(query.PageNumber, query.PageSize);
            
        return assets.ConvertAll(asset => new AssetShortResponse( 
                Id: asset.Id.Value,
                Name: asset.Name,
                Description: asset.Description,
                FileFormat: asset.FileFormat,
                FileSize: asset.FileSize,
                Path: asset.Path,
                Briefing: new BriefingResponse(
                    CreatedBy: asset.Briefing.CreatedBy,
                    CreatedDate: asset.Briefing.CreatedDate
                )
            )
        );
    }
}