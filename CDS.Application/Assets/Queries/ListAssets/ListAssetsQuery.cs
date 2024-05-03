using CDS.Domain.AssetAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.ListAssets;

public record ListAssetsQuery : IRequest<ErrorOr<List<Asset>>>;