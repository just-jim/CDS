using CDS.Domain.ContentDistributionAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.ContentDistributions.Commands.CreateContentDistribution;

public record CreateContentDistributionCommand(
    DateOnly DistributionDate,
    string DistributionChannel,
    string DistributionMethod
) : IRequest<ErrorOr<ContentDistribution>>;