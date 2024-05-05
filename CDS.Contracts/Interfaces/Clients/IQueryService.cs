namespace CDS.Contracts.Interfaces.Clients;

public interface IQueryService {
    Task<IQueryResponse?> FetchDataAsync(string entityId);
}