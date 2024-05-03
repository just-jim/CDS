using CDS.Domain.AssetAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.GetAsset;

public record GetAssetQuery(string AssetId) : IRequest<ErrorOr<Asset>>;