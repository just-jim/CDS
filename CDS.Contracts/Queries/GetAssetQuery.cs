using ErrorOr;
using MediatR;

namespace CDS.Contracts.Queries;

public record GetAssetQuery(string AssetId) : IRequest<ErrorOr<string>>;