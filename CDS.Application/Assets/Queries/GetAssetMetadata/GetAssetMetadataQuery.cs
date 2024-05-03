using CDS.Contracts;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.GetAssetMetadata;

public record GetAssetMetadataQuery(string AssetId) : IRequest<ErrorOr<AssetResponse>>;
