using CDS.Domain.ContentDistributionAggregate;

namespace CDS.Application.Common.Interfaces.Database;

public interface IContentDistributionRepository {
    Task AddAsync(ContentDistribution contentDistribution);
}