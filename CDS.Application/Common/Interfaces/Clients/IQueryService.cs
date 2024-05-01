using CDS.Application.Common.Interfaces.Models;

namespace CDS.Application.Common.Interfaces.Clients;

public interface IQueryService {
    Task<IMessage?> FetchDataAsync(string entityId);
}