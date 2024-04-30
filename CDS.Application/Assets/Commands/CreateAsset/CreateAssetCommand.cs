using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.Entities;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Commands.CreateAsset;

public record CreateAssetCommand(
    string AssetId,
    string Name,
    string Description,
    string FileFormat,
    string FileSize,
    string Path
) : IRequest<ErrorOr<Asset>>;

public record CreateBriefingCommand(
    string AssetId,
    string Name,
    string Description
): IRequest<ErrorOr<Briefing>>;

public record CreateAssetOrderCommand(
    string AssetId,
    string OrderNumber,
    int Quantity
): IRequest<ErrorOr<AssetOrder>>;
    
public record CreateAssetContentDistributionCommand(
    string AssetId,
    Guid ContentDistributionId,
    string FileUrl
): IRequest<ErrorOr<AssetContentDistribution>>;