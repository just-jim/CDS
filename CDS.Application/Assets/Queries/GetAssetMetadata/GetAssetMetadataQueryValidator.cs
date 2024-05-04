using FluentValidation;

namespace CDS.Application.Assets.Queries.GetAssetMetadata;

public class GetAssetMetadataQueryValidator : AbstractValidator<GetAssetMetadataQuery> {
    public GetAssetMetadataQueryValidator() {
        RuleFor(x => x.AssetId).NotEmpty();
    }
}