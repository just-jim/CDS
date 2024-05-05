using CDS.Contracts.Commands;
using FluentValidation;

namespace CDS.Application.Assets.Commands.CreateAsset;

public class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand> {
    public CreateAssetCommandValidator() {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}