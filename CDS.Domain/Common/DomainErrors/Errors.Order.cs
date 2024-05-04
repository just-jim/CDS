using ErrorOr;

namespace CDS.Domain.Common.DomainErrors;

public static partial class Errors {
    public static class OrderError {
        public static Error NotFound {
            get => Error.NotFound(
                "Order.NotFound",
                "Order with the given number does not exist");
        }

        public static Error AlreadyExists {
            get => Error.Conflict(
                "Order.AlreadyExists",
                "Order with the given number already exists");
        }
    }
}