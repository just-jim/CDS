using ErrorOr;

namespace CDS.Domain.Common.DomainErrors;

public static partial class Errors {
    public static class AssetError {
        public static Error NotFound {
            get => Error.NotFound(
                "Asset.NotFound",
                "Asset with given ID does not exist");
        }

        public static Error FileUrlNotFound {
            get => Error.NotFound(
                "AssetFileUrl.NotFound",
                "No content distribution fileUrl was found for the asset");
        }
    }
}