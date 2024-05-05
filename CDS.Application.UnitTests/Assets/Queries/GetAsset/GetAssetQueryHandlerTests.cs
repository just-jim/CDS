using CDS.Application.Assets.Queries.GetAsset;
using CDS.Contracts.Interfaces.Cache;
using CDS.Contracts.Interfaces.Database;
using CDS.Contracts.Models;
using CDS.Contracts.Models.Cache;
using CDS.Contracts.Queries;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.Entities;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;

namespace CDS.Application.UnitTests.Assets.Queries.GetAsset;

public class GetAssetQueryHandlerTests {

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenAssetDoesNotExist() {
        // Arrange
        Mock<IAssetRepository> mockAssetRepository = new Mock<IAssetRepository>();
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<GetAssetQueryHandler>> mockLogger = new Mock<ILogger<GetAssetQueryHandler>>();
        var handler = new GetAssetQueryHandler(mockAssetRepository.Object, mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        var query = new GetAssetQuery("nonexistentId");

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(false);

        // Act
        ErrorOr<string> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.AssetError.NotFound, result.FirstError);
    }

    [Fact]
    public async Task Handle_ReturnsFileUrlFromCache_WhenFileUrlIsCached() {
        // Arrange
        Mock<IAssetRepository> mockAssetRepository = new Mock<IAssetRepository>();
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<GetAssetQueryHandler>> mockLogger = new Mock<ILogger<GetAssetQueryHandler>>();
        var handler = new GetAssetQueryHandler(mockAssetRepository.Object, mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        var query = new GetAssetQuery("assetId");
        var cachedUrl = new AssetFileUrlCache("https://cachedUrl.com", DateOnly.Parse("2024-01-01"));

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(true);
        mockCache.Setup(c => c.Get(It.IsAny<string>(), It.IsAny<Type>())).Returns(cachedUrl);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(cachedUrl.FileUrl, result.Value);
    }

    [Fact]
    public async Task Handle_ReturnsFileUrlFromContentDistribution_WhenFileUrlIsNotCached() {
        // Arrange
        Mock<IAssetRepository> mockAssetRepository = new Mock<IAssetRepository>();
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<GetAssetQueryHandler>> mockLogger = new Mock<ILogger<GetAssetQueryHandler>>();
        var handler = new GetAssetQueryHandler(mockAssetRepository.Object, mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        var query = new GetAssetQuery("assetId");
        var assetContentDistribution = AssetContentDistribution.Create(AssetId.Create("assetId"), "fileUrl");
        var contentDistribution = ContentDistribution.Create(
            DateOnly.Parse("2024-01-01"),
            "distributionName",
            "distributionDescription",
            [assetContentDistribution]
        );

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(true);
        mockCache.Setup(c => c.Get(It.IsAny<string>(), It.IsAny<Type>())).Returns((AssetFileUrlCache?)null);
        mockContentDistributionRepository.Setup(r => r.GetMostRecentContentDistributionForAnAssetIdAsync(It.IsAny<AssetId>())).ReturnsAsync(contentDistribution);

        // Act
        ErrorOr<string> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(contentDistribution.AssetContentDistributions[0].FileUrl, result.Value);
    }

    [Fact]
    public async Task Handle_ReturnsFileUrlFromContentDistribution_WhenFileUrlIsNotCachedAndNotPersisted() {
        // Arrange
        Mock<IAssetRepository> mockAssetRepository = new Mock<IAssetRepository>();
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<GetAssetQueryHandler>> mockLogger = new Mock<ILogger<GetAssetQueryHandler>>();
        var handler = new GetAssetQueryHandler(mockAssetRepository.Object, mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        var query = new GetAssetQuery("assetId");

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(true);
        mockCache.Setup(c => c.Get(It.IsAny<string>(), It.IsAny<Type>())).Returns((AssetFileUrlCache?)null);
        mockContentDistributionRepository.Setup(r => r.GetMostRecentContentDistributionForAnAssetIdAsync(It.IsAny<AssetId>())).ReturnsAsync((ContentDistribution?)null);

        // Act
        ErrorOr<string> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(Errors.AssetError.FileUrlNotFound, result.FirstError);
    }

}