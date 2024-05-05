using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.Events;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;

namespace CDS.Domain.AssetAggregate;

public class Asset : AggregateRoot<AssetId, string> {
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string FileFormat { get; private set; }
    public string FileSize { get; private set; }
    public string Path { get; private set; }
    public Briefing Briefing { get; private set; }

    Asset(
        AssetId assetId,
        string name,
        string description,
        string fileFormat,
        string fileSize,
        string path,
        Briefing briefing
    ) : base(assetId) {
        Name = name;
        Description = description;
        FileFormat = fileFormat;
        FileSize = fileSize;
        Path = path;
        Briefing = briefing;
    }

    public static Asset Create(
        AssetId assetId,
        string name,
        string description,
        string fileFormat,
        string fileSize,
        string path,
        Briefing briefing
    ) {
        // Define invariants
        if (string.IsNullOrWhiteSpace(assetId.Value)) {
            throw new ArgumentNullException(nameof(assetId), "An Asset must have an non empty AssetId");
        }
        if (string.IsNullOrWhiteSpace(name)) {
            throw new InvalidOperationException("An asset must have a name.");
        }
        
        var asset = new Asset(
            assetId,
            name,
            description,
            fileFormat,
            fileSize,
            path,
            briefing
        );
        asset.AddDomainEvent(new AssetCreated(asset));
        return asset;
    }

    public void Update(
        string name,
        string description,
        string fileFormat,
        string fileSize,
        string path,
        Briefing briefing
    ) {
        Name = name;
        Description = description;
        FileFormat = fileFormat;
        FileSize = fileSize;
        Path = path;
        Briefing = briefing;

        AddDomainEvent(new AssetUpdated(this));
    }

#pragma warning disable CS8618
    Asset() { }
#pragma warning restore CS8618
}