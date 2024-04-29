using ErrorOr;

namespace CDS.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Order
    {
        public static Error NotFound {
            get => Error.NotFound(
                code: "Order.NotFound",
                description: "Order with the given number does not exist");
        }
        
        public static Error AlreadyExists {
            get => Error.Conflict(
                code: "Order.AlreadyExists",
                description: "Order with the given number already exists");
        }
    }
}