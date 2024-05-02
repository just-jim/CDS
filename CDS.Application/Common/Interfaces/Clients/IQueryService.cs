namespace CDS.Application.Common.Interfaces.Clients;

public interface IQueryService {
    Task<IQueryResponse?> FetchDataAsync(string entityId);
}