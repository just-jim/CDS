using ErrorOr;
using MediatR;

namespace CDS.Application.Assets.Queries.Admin;

public record ResetQuery() : IRequest<ErrorOr<bool>>;
public record DropDbQuery() : IRequest<ErrorOr<bool>>;
public record PurgeCacheQuery() : IRequest<bool>;