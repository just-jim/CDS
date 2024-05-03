using CDS.Application.Common.Interfaces.Clients;
using CDS.Application.Common.Interfaces.Database;
using CDS.Application.Common.Models;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Commands.CreateAsset;

public class CreateAssetCommandHandler(IAssetRepository assetRepository, IQueryService briefingQueryService) : IRequestHandler<CreateAssetCommand, ErrorOr<Asset>> {

    public async Task<ErrorOr<Asset>> Handle(CreateAssetCommand request, CancellationToken ct) {
        var assetId = AssetId.Create(request.AssetId);
        if (await assetRepository.ExistsAsync(assetId)) {
            return Errors.AssetError.AlreadyExists;
        }
        
        var briefingDomainBriefing = (BriefingDomainBriefing?) await briefingQueryService.FetchDataAsync(request.Name);
        var briefing = briefingDomainBriefing != null 
            ? Briefing.Create(briefingDomainBriefing.CreatedBy,DateOnly.Parse(briefingDomainBriefing.CreatedDate))
            : Briefing.Create(null,null);
        
        var asset = Asset.Create(
            assetId: assetId,
            name: request.Name,
            description: request.Description,
            fileFormat: request.FileFormat,
            fileSize: request.FileSize,
            path: request.Path,
            briefing: briefing
        );
        
        await assetRepository.AddAsync(asset);

        return asset;
    }
}