using CDS.Domain.ContentDistributionAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Contracts.Commands;

public record CreateContentDistributionCommand(
    DateOnly DistributionDate,
    string DistributionChannel,
    string DistributionMethod,
    List<AssetContentDistributionCommand> AssetContentDistributions
) : IRequest<ErrorOr<ContentDistribution>>;
public record AssetContentDistributionCommand(
    string AssetId,
    string FileUrl
);