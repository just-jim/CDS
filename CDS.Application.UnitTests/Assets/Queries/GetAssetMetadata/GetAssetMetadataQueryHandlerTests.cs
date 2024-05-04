using CDS.Application.Assets.Queries.GetAssetMetadata;
using CDS.Application.Common.Interfaces.Database;
using CDS.Contracts;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.Entities;
using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.Entities;
using ErrorOr;
using Moq;

namespace CDS.Application.UnitTests.Assets.Queries.GetAssetMetadata;

public class GetAssetMetadataQueryHandlerTests {
    
    [Fact]
    public async Task Handle_ShouldReturnAssetResponse_WhenAssetExists() {
        // Arrange
        Mock<IAssetRepository> mockAssetRepository = new Mock<IAssetRepository>();
        Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        var handler = new GetAssetMetadataQueryHandler(mockAssetRepository.Object, mockOrderRepository.Object, mockContentDistributionRepository.Object);
        var query = new GetAssetMetadataQuery("existingId");
        var existingAsset = Asset.Create(
            AssetId.Create("existingId"),
            "param2",
            "param3",
            "param4",
            "param5",
            "param6",
            Briefing.Create("briefingParam", DateOnly.Parse("2024-01-01")
            )
        );

        List<Order> orders = [
            Order.Create(
                "orderNumber",
                "customerName",
                DateOnly.Parse("2024-01-01"),
                10,
                [AssetOrder.Create(AssetId.Create("existingId"), 1)]
            ),

            Order.Create(
                "orderNumber2",
                "customerName2",
                DateOnly.Parse("2024-01-02"),
                8,
                [AssetOrder.Create(AssetId.Create("existingId"), 3)]
            )
        ];

        List<ContentDistribution> contentDistributions = [
            ContentDistribution.Create(
                DateOnly.Parse("2024-01-01"),
                "distributionChannel",
                "distributionMethod",
                [AssetContentDistribution.Create(AssetId.Create("existingId"), "fileUrl")]
            ),
            ContentDistribution.Create(
                DateOnly.Parse("2024-01-02"),
                "distributionChannel2",
                "distributionMethod2",
                [AssetContentDistribution.Create(AssetId.Create("existingId"), "fileUrl2")]
            )
        ];

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(true);
        mockAssetRepository.Setup(r => r.GetByIdAsync(It.IsAny<AssetId>())).ReturnsAsync(existingAsset);
        mockOrderRepository.Setup(r => r.FindOrdersByAssetId(It.IsAny<AssetId>())).ReturnsAsync(orders);
        mockContentDistributionRepository.Setup(r => r.FindContentDistributionsByAssetId(It.IsAny<AssetId>())).ReturnsAsync(contentDistributions);

        var expectedAssetResponse = new AssetResponse(
            existingAsset.Id.Value,
            existingAsset.Name,
            existingAsset.Description,
            existingAsset.FileFormat,
            existingAsset.FileSize,
            existingAsset.Path,
            new BriefingResponse(existingAsset.Briefing.CreatedBy, existingAsset.Briefing.CreatedDate),
            orders.Select(order => new AssetOrderResponse(
                order.Id.Value,
                order.AssetOrders.First(ao => ao.AssetId == existingAsset.Id).Quantity,
                order.CustomerName,
                order.OrderDate,
                order.TotalAssets
            )).ToList(),
            contentDistributions.Select(cd => new AssetContentDistributionResponse(
                cd.AssetContentDistributions.First(acd => acd.AssetId == existingAsset.Id).FileUrl,
                cd.DistributionDate,
                cd.DistributionChannel,
                cd.DistributionMethod
            )).ToList()
        );

        // Act
        ErrorOr<AssetResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedAssetResponse.Id, result.Value.Id);
        Assert.Equal(expectedAssetResponse.Name, result.Value.Name);
        Assert.Equal(expectedAssetResponse.Description, result.Value.Description);
        Assert.Equal(expectedAssetResponse.FileFormat, result.Value.FileFormat);
        Assert.Equal(expectedAssetResponse.FileSize, result.Value.FileSize);
        Assert.Equal(expectedAssetResponse.Path, result.Value.Path);
        Assert.Equal(expectedAssetResponse.Briefing.CreatedBy, result.Value.Briefing.CreatedBy);
        Assert.Equal(expectedAssetResponse.Briefing.CreatedDate, result.Value.Briefing.CreatedDate);
        Assert.Equal(expectedAssetResponse.Orders[0].OrderId, result.Value.Orders[0].OrderId);
        Assert.Equal(expectedAssetResponse.Orders[0].OrderDate, result.Value.Orders[0].OrderDate);
        Assert.Equal(expectedAssetResponse.Orders[0].CustomerName, result.Value.Orders[0].CustomerName);
        Assert.Equal(expectedAssetResponse.Orders[0].OrderTotalAssets, result.Value.Orders[0].OrderTotalAssets);
        Assert.Equal(expectedAssetResponse.Orders[0].Quantity, result.Value.Orders[0].Quantity);
        Assert.Equal(expectedAssetResponse.Orders[1].OrderId, result.Value.Orders[1].OrderId);
        Assert.Equal(expectedAssetResponse.Orders[1].OrderDate, result.Value.Orders[1].OrderDate);
        Assert.Equal(expectedAssetResponse.Orders[1].CustomerName, result.Value.Orders[1].CustomerName);
        Assert.Equal(expectedAssetResponse.Orders[1].OrderTotalAssets, result.Value.Orders[1].OrderTotalAssets);
        Assert.Equal(expectedAssetResponse.Orders[1].Quantity, result.Value.Orders[1].Quantity);
        Assert.Equal(expectedAssetResponse.ContentDistributions[0].FileUrl, result.Value.ContentDistributions[0].FileUrl);
        Assert.Equal(expectedAssetResponse.ContentDistributions[0].DistributionDate, result.Value.ContentDistributions[0].DistributionDate);
        Assert.Equal(expectedAssetResponse.ContentDistributions[0].DistributionChannel, result.Value.ContentDistributions[0].DistributionChannel);
        Assert.Equal(expectedAssetResponse.ContentDistributions[0].DistributionMethod, result.Value.ContentDistributions[0].DistributionMethod);
        Assert.Equal(expectedAssetResponse.ContentDistributions[1].FileUrl, result.Value.ContentDistributions[1].FileUrl);
        Assert.Equal(expectedAssetResponse.ContentDistributions[1].DistributionDate, result.Value.ContentDistributions[1].DistributionDate);
        Assert.Equal(expectedAssetResponse.ContentDistributions[1].DistributionChannel, result.Value.ContentDistributions[1].DistributionChannel);
        Assert.Equal(expectedAssetResponse.ContentDistributions[1].DistributionMethod, result.Value.ContentDistributions[1].DistributionMethod);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenAssetDoesNotExist() {
        // Arrange
        Mock<IAssetRepository> mockAssetRepository = new Mock<IAssetRepository>();
        Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        var handler = new GetAssetMetadataQueryHandler(mockAssetRepository.Object, mockOrderRepository.Object, mockContentDistributionRepository.Object);
        var query = new GetAssetMetadataQuery("existingId");

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(false);

        // Act
        ErrorOr<AssetResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.AssetError.NotFound, result.FirstError);
    }
}