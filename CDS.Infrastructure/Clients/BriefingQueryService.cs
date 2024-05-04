using System.Text.Json;
using CDS.Application.Common.Interfaces.Clients;
using CDS.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.Clients;

public class BriefingQueryService(HttpClient httpClient, IConfiguration configuration, ILogger<BriefingQueryService> logger) : IQueryService {

    readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    readonly string? _baseUrl = configuration.GetConnectionString("BriefingDomain");

    public async Task<IQueryResponse?> FetchDataAsync(string briefingName) {
        string url = $"{_baseUrl}/briefings/{briefingName}";
        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) {
            logger.LogError($"Failed to fetch the Briefing for the name: '{briefingName}'");
            // TODO We can add a retry mechanism here if it's a network error
            return null;
        }

        try {
            string json = await response.Content.ReadAsStringAsync();
            var briefing = JsonSerializer.Deserialize<BriefingDomainBriefing>(json, _jsonSerializerOptions);
            if (briefing == null) {
                throw new JsonException("Briefing object deserialized to null");
            }
            logger.LogInformation($"Briefing for name {briefing.Name} fetched from the Briefing domain");
            return briefing;
        }
        catch (JsonException e) {
            logger.LogError($"Briefing object fetched from Briefing domain for name {briefingName} was malformed", e);
        }

        return null;
    }
}