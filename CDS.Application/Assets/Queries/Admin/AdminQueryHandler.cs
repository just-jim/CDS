using CDS.Application.Common.Interfaces.Cache;
using CDS.Application.Common.Interfaces.Database;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CDS.Application.Assets.Queries.Admin;

public class AdminQueryHandler(
    IAssetRepository assetRepository,
    IContentDistributionRepository contentDistributionRepository,
    IOrderRepository orderRepository,
    ICacheService cache,
    ILogger<AdminQueryHandler> logger
) : IRequestHandler<ResetQuery, ErrorOr<bool>>,
    IRequestHandler<DropDbQuery, ErrorOr<bool>>,
    IRequestHandler<PurgeCacheQuery, bool> {

    public async Task<ErrorOr<bool>> Handle(ResetQuery query, CancellationToken ct) {
        await DropDb();
        cache.Purge();

        return true;
    }

    public async Task<ErrorOr<bool>> Handle(DropDbQuery query, CancellationToken ct) {
        await DropDb();
        return true;
    }

    public Task<bool> Handle(PurgeCacheQuery query, CancellationToken ct) {
        cache.Purge();
        return Task.FromResult(true);
    }

    async Task DropDb() {
        await assetRepository.ResetAsync();
        await contentDistributionRepository.ResetAsync();
        await orderRepository.ResetAsync();
    }
}