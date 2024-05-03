namespace CDS.Contracts;

public record AssetResponse(
    string Id,
    string Name,
    string Description,
    string? FileFormat,
    string? FileSize,
    string? Path,
    BriefingResponse Briefing,
    List<AssetOrderResponse> Orders,
    List<AssetContentDistributionResponse> ContentDistributions
);

public record AssetOrderResponse(
    string OrderId,
    string Quantity,
    string CustomerName,
    DateOnly OrderDate,
    int TotalAssets
);

public record AssetContentDistributionResponse(
    string FileUrl,
    DateOnly DistributionDate,
    string DistributionChannel,
    string DistributionMethod
);
    
public record BriefingResponse(
    string? CreatedBy,
    DateOnly? CreatedDate
);