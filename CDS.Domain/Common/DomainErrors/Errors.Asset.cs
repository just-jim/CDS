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
        
        public static Error FileUrlNotFound {
            get => Error.NotFound(
                code: "AssetFileUrl.NotFound",
                description: "No content distribution fileUrl was found for the asset");
        }
    }
}