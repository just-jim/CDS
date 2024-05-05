using CDS.Contracts.Models.Responses;
using ErrorOr;
using MediatR;

namespace CDS.Contracts.Queries;

public record GetAssetMetadataQuery(string AssetId) : IRequest<ErrorOr<AssetResponse>>;