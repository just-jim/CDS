namespace Mock.Asset.Models;

public record Asset(
    string AssetId,
    string Name,
    string Description,
    string FileFormat,
    string FileSize,
    string Path
);