using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.Entities;
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
            distributionMethod: request.DistributionMethod,
            assetContentDistributions: request.AssetContentDistributions.ConvertAll(assetContentDistribution => AssetContentDistribution.Create(
                AssetId.Create(assetContentDistribution.AssetId),
                assetContentDistribution.FileUrl
                )
            )
        );
        
        await contentDistributionRepository.AddAsync(contentDistribution);

        return contentDistribution;
    }
}