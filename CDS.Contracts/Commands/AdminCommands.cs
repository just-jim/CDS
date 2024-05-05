using ErrorOr;
using MediatR;

namespace CDS.Contracts.Commands;

public record ResetCommand : IRequest<ErrorOr<bool>>;
public record DropDbCommand : IRequest<ErrorOr<bool>>;
public record PurgeCacheCommand : IRequest<bool>;