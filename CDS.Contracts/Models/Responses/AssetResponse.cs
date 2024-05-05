namespace CDS.Contracts.Models.Responses;

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
public record AssetShortResponse(
    string Id,
    string Name,
    string Description,
    string? FileFormat,
    string? FileSize,
    string? Path,
    BriefingResponse Briefing
);
public record AssetOrderResponse(
    string OrderId,
    int Quantity,
    string CustomerName,
    DateOnly OrderDate,
    int OrderTotalAssets
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