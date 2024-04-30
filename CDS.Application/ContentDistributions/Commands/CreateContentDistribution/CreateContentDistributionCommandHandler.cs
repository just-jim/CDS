using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.ContentDistributionAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.ContentDistributions.Commands.CreateContentDistribution;

public class CreateContentDistributionCommandHandler(
    IContentDistributionRepository contentDistributionRepository
    ) : IRequestHandler<CreateContentDistributionCommand, ErrorOr<ContentDistribution>> {
    public async Task<ErrorOr<ContentDistribution>> Handle(CreateContentDistributionCommand request, CancellationToken ct) {
        var contentDistribution = ContentDistribution.Create(
            distributionDate: request.DistributionDate,
            distributionChannel: request.DistributionChannel,
            distributionMethod: request.DistributionMethod
        );
        
        await contentDistributionRepository.AddAsync(contentDistribution);

        return contentDistribution;
    }
}