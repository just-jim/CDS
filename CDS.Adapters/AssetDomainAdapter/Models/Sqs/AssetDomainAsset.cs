namespace CDS.Adapters.AssetDomainAdapter.SqsModels;

public record AssetDomainAsset(
    string AssetId,
    string Name,
    string Description,
    string FileFormat,
    string FileSize,
    string Path
);