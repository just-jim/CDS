using System.Text.Json;
using Mock.Content.Distribution.Models;

namespace Mock.Content.Distribution.Controllers;

internal class ContentDistributionController {
    
    static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };

    static ContentDistribution ParseJson() {
        const string jsonFilePath = "Resources/ContentDistributionMetadata.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<ContentDistribution>(jsonString, JsonSerializerOptions)!;
    }

    public ContentDistribution Cache() {
        return ParseJson();
    }
}