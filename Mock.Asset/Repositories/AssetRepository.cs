using System.Text.Json;

namespace Mock.Asset.Repositories;

internal class AssetRepository {
    readonly Dictionary<string, Models.Asset> _assets = new Dictionary<string, Models.Asset>();

    public AssetRepository() {
        const string jsonFilePath = "Resources/AssetMetadata.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        foreach (var asset in JsonSerializer.Deserialize<List<Models.Asset>>(jsonString, jsonSerializerOptions)!) {
            Create(asset);
        }
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

    public void Update(Models.Asset asset) {
        var existingAsset = GetById(asset.AssetId);
        if (existingAsset is null) {
            return;
        }

        _assets[asset.AssetId] = asset;
    }

    public void Delete(string id) {
        _assets.Remove(id);
    }
}