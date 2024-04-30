using CDS.Domain.AssetAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Commands.CreateAsset;

public record CreateAssetCommand(
    string AssetId,
    string Name,
    string Description,
    string FileFormat,
    string FileSize,
    string Path,
    BriefingCommand Briefing
) : IRequest<ErrorOr<Asset>>;

public record BriefingCommand(
    string? CreatedBy,
    DateOnly? CreatedDate
);