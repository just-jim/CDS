using ErrorOr;

namespace CDS.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class ContentDistributionError
    {
        public static Error NotFound {
            get => Error.NotFound(
                code: "ContentDistribution.NotFound",
                description: "ContentDistribution with the given ID does not exist");
        }
    }
}