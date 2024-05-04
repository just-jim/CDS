using FluentValidation;

namespace CDS.Application.ContentDistributions.Commands.CreateContentDistribution;

public class CreateContentDistributionCommandValidator : AbstractValidator<CreateContentDistributionCommand> {
    public CreateContentDistributionCommandValidator() {
        RuleFor(x => x.DistributionDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
    }
}