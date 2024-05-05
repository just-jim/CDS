using CDS.Contracts.Commands;
using CDS.Contracts.Interfaces.Cache;
using CDS.Contracts.Interfaces.Database;
using ErrorOr;
using MediatR;

namespace CDS.Application.Admin.Commands;

public class AdminCommandsHandler(
    IAssetRepository assetRepository,
    IContentDistributionRepository contentDistributionRepository,
    IOrderRepository orderRepository,
    ICacheService cache
) : IRequestHandler<ResetCommand, ErrorOr<bool>>,
    IRequestHandler<DropDbCommand, ErrorOr<bool>>,
    IRequestHandler<PurgeCacheCommand, bool> {

    public async Task<ErrorOr<bool>> Handle(ResetCommand command, CancellationToken ct) {
        await DropDb();
        cache.Purge();

        return true;
    }

    public async Task<ErrorOr<bool>> Handle(DropDbCommand command, CancellationToken ct) {
        await DropDb();
        return true;
    }

    public Task<bool> Handle(PurgeCacheCommand command, CancellationToken ct) {
        cache.Purge();
        return Task.FromResult(true);
    }

    async Task DropDb() {
        await assetRepository.ResetAsync();
        await contentDistributionRepository.ResetAsync();
        await orderRepository.ResetAsync();
    }
}