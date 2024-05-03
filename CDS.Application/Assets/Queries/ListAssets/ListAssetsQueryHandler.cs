using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.ListAssets;

public class ListAssetsQueryHandler(IAssetRepository assetRepository) : IRequestHandler<ListAssetsQuery, ErrorOr<List<Asset>>> {

    public async Task<ErrorOr<List<Asset>>> Handle(ListAssetsQuery query, CancellationToken ct)
    {
        return await assetRepository.ListAsync();
    }
}