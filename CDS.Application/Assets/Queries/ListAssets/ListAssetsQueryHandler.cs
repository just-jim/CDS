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
                asset.Id.Value,
                asset.Name,
                asset.Description,
                asset.FileFormat,
                asset.FileSize,
                asset.Path,
                new BriefingResponse(
                    asset.Briefing.CreatedBy,
                    asset.Briefing.CreatedDate
                )
            )
        );
    }
}