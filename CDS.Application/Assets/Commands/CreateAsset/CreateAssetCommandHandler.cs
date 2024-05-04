using CDS.Application.Common.Interfaces.Clients;
using CDS.Application.Common.Interfaces.Database;
using CDS.Application.Common.Models;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.ValueObjects;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CDS.Application.Assets.Commands.CreateAsset;

public class CreateAssetCommandHandler(
    IAssetRepository assetRepository,
    IQueryService briefingQueryService,
    ILogger<CreateAssetCommandHandler> logger
) : IRequestHandler<CreateAssetCommand, ErrorOr<Asset>> {

    public async Task<ErrorOr<Asset>> Handle(CreateAssetCommand request, CancellationToken ct) {
        var assetId = AssetId.Create(request.AssetId);

        var briefingDomainBriefing = (BriefingDomainBriefing?)await briefingQueryService.FetchDataAsync(request.Name);
        var briefing = briefingDomainBriefing != null ? Briefing.Create(briefingDomainBriefing.CreatedBy, DateOnly.Parse(briefingDomainBriefing.CreatedDate)) : Briefing.Create(null, null);

        if (await assetRepository.ExistsAsync(assetId)) {
            var asset = await assetRepository.GetByIdAsync(assetId);
            asset!.Update(
                request.Name,
                request.Description,
                request.FileFormat,
                request.FileSize,
                request.Path,
                briefing
            );
            await assetRepository.UpdateAsync(asset);
            logger.LogInformation($"Asset with id '{assetId.Value}' updated");
            return asset;
        }
        else {
            var asset = Asset.Create(
                assetId,
                request.Name,
                request.Description,
                request.FileFormat,
                request.FileSize,
                request.Path,
                briefing
            );
            await assetRepository.AddAsync(asset);
            return asset;
        }
    }
}