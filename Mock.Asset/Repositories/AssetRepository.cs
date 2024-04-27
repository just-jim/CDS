using System.Text.Json;

namespace Mock.Asset.Repositories;

internal class AssetRepository {
    readonly Dictionary<string, Models.Asset> _assets = new Dictionary<string, Models.Asset>();

    public AssetRepository(ILogger<AssetRepository> logger) {
        const string jsonFilePath = "Resources/AssetMetadata.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (var asset in JsonSerializer.Deserialize<List<Models.Asset>>(jsonString, jsonSerializerOptions)!) {
            Create(asset);
        }
        logger.LogInformation("Asset repository initialized");
    }

    void Create(Models.Asset asset) {
        _assets[asset.AssetId] = asset;
    }

    public Models.Asset? GetById(string id) {
        return _assets.GetValueOrDefault(id);
    }

    public List<Models.Asset> GetAll() {
        return _assets.Values.ToList();
    }
}