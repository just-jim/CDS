using ErrorOr;

namespace CDS.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class AssetError
    {
        public static Error NotFound {
            get => Error.NotFound(
                code: "Asset.NotFound",
                description: "Asset with given ID does not exist");
        }
        
        public static Error AlreadyExists {
            get => Error.Conflict(
                code: "Asset.AlreadyExists",
                description: "Asset with the given ID already exists");
        }
    }
}