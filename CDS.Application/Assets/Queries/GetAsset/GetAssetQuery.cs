using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.GetAsset;

public record GetAssetQuery(string AssetId) : IRequest<ErrorOr<string>>;