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
    string Path
) : IRequest<ErrorOr<Asset>>;