using System.Text.Json;

namespace Mock.Briefing.Repositories;

internal class BriefingRepository {
    readonly Dictionary<string, Models.Briefing> _briefings = new Dictionary<string, Models.Briefing>();

    public BriefingRepository(ILogger<BriefingRepository> logger) {
        const string jsonFilePath = "Resources/BriefingMetadata.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (var briefing in JsonSerializer.Deserialize<List<Models.Briefing>>(jsonString, jsonSerializerOptions)!) {
            Create(briefing);
        }
        
        logger.LogInformation("Briefing repository initialized");
    }

    void Create(Models.Briefing briefing) {
        _briefings[briefing.Name] = briefing;
    }

    public Models.Briefing? GetByName(string id) {
        return _briefings.GetValueOrDefault(id);
    }

    public List<Models.Briefing> GetAll() {
        return _briefings.Values.ToList();
    }
}