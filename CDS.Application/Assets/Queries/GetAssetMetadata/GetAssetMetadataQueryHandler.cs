using CDS.Application.Common.Interfaces.Database;
using CDS.Contracts;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.OrderAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.GetAssetMetadata;

public class GetAssetMetadataQueryHandler(
    IAssetRepository assetRepository,
    IOrderRepository orderRepository,
    IContentDistributionRepository contentDistributionRepository
) : IRequestHandler<GetAssetMetadataQuery, ErrorOr<AssetResponse>> {
    public async Task<ErrorOr<AssetResponse>> Handle(GetAssetMetadataQuery query, CancellationToken ct) {
        var assetId = AssetId.Create(query.AssetId);

        if (!await assetRepository.ExistsAsync(assetId)) {
            return Errors.AssetError.NotFound;
        }

        Asset asset = (await assetRepository.GetByIdAsync(assetId))!;
        List<Order> orders = await orderRepository.FindOrdersByAssetId(assetId);
        List<ContentDistribution> contentDistributions = await contentDistributionRepository.FindContentDistributionsByAssetId(assetId);

        List<AssetOrderResponse> assetOrderResponses = orders
            .Select(order => new AssetOrderResponse(
                order.Id.Value,
                order.AssetOrders.First(ao => ao.AssetId == assetId).Quantity,
                order.CustomerName,
                order.OrderDate,
                order.TotalAssets
            )).ToList();

        List<AssetContentDistributionResponse> assetContentDistributionResponses = contentDistributions
            .Select(cd => new AssetContentDistributionResponse(
                cd.AssetContentDistributions.First(acd => acd.AssetId == assetId).FileUrl,
                cd.DistributionDate,
                cd.DistributionChannel,
                cd.DistributionMethod
            )).ToList();

        var assetResponse = new AssetResponse(
            asset.Id.Value,
            asset.Name,
            asset.Description,
            asset.FileFormat,
            asset.FileSize,
            asset.Path,
            new BriefingResponse(asset.Briefing.CreatedBy, asset.Briefing.CreatedDate),
            assetOrderResponses,
            assetContentDistributionResponses
        );

        return assetResponse;
    }
}