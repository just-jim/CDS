using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Commands.CreateAsset;

public class CreateAssetCommandHandler(IAssetRepository assetRepository) : IRequestHandler<CreateAssetCommand, ErrorOr<Asset>> {

    public async Task<ErrorOr<Asset>> Handle(CreateAssetCommand request, CancellationToken ct) {
        var assetId = AssetId.Create(request.AssetId);
        if (await assetRepository.ExistsAsync(assetId)) {
            return Errors.Asset.AlreadyExists;
        }
        
        var asset = Asset.Create(
            assetId: assetId,
            name: request.Name,
            description: request.Description,
            fileFormat: request.FileFormat,
            fileSize: request.FileSize,
            path: request.Path,
            briefing: Briefing.Create(request.Briefing.CreatedBy,request.Briefing.CreatedDate)
        );
        
        await assetRepository.AddAsync(asset);

        return asset;
    }
}