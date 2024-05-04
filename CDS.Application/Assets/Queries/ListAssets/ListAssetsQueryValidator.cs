using FluentValidation;

namespace CDS.Application.Assets.Queries.ListAssets;

public class ListAssetsQueryValidator : AbstractValidator<ListAssetsQuery> {
    public ListAssetsQueryValidator() {
        RuleFor(x => x.PageSize).LessThan(100);
    }
}