using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.ContentDistributionAggregate;

namespace CDS.Infrastructure.Database.Repositories;

public class ContentDistributionRepository(CdsDbContext dbContext) : IContentDistributionRepository {
    public async Task AddAsync(ContentDistribution contentDistribution) {
        dbContext.Add(contentDistribution);
        await dbContext.SaveChangesAsync();
    }
}