using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.Events;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;

namespace CDS.Domain.AssetAggregate;

public class Asset : AggregateRoot<AssetId, string> {
    readonly List<AssetOrder> _assetOrders = [];
    readonly List<AssetContentDistribution> _assetContentDistributions = [];

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string FileFormat { get; private set; }
    public string FileSize { get; private set; }
    public string Path { get; private set; }
    public Briefing? Briefing { get; private set; }
    public IReadOnlyList<AssetOrder> AssetOrders {
        get => _assetOrders.AsReadOnly();
    }
    public IReadOnlyList<AssetContentDistribution> AssetContentDistributions {
        get => _assetContentDistributions.AsReadOnly();
    }

    Asset(
        AssetId assetId,
        string name,
        string description,
        string fileFormat,
        string fileSize,
        string path
    ) : base(assetId) {
        Name = name;
        Description = description;
        FileFormat = fileFormat;
        FileSize = fileSize;
        Path = path;
    }

    public static Asset Create(
        AssetId assetId,
        string name,
        string description,
        string fileFormat,
        string fileSize,
        string path
    ) {
        var asset = new Asset(
            assetId,
            name,
            description,
            fileFormat,
            fileSize,
            path
        );
        asset.AddDomainEvent(new AssetCreated(asset));
        return asset;
    }

    public void AddBriefing(Briefing briefing) {
        Briefing = briefing;
    }

    public void AddAssetOrder(AssetOrder assetOrder) {
        _assetOrders.Add(assetOrder);
    }

    public void AddAssetContentDistribution(AssetContentDistribution assetContentDistribution) {
        _assetContentDistributions.Add(assetContentDistribution);
    }
    
#pragma warning disable CS8618
    Asset() { }
#pragma warning restore CS8618
}