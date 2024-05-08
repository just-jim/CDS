using CDS.Application.ContentDistributions.Commands.CreateContentDistribution;
using CDS.Contracts.Commands;
using CDS.Contracts.Interfaces.Cache;
using CDS.Contracts.Interfaces.Database;
using CDS.Contracts.Models.Cache;
using CDS.Domain.ContentDistributionAggregate;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;

namespace CDS.Application.UnitTests.ContentDistributions.Commands.CreateContentDistribution;

public class CreateContentDistributionCommandHandlerTests {

    [Fact]
    public async Task Handle_ShouldCreateContentDistribution_WhenValidRequestIsProvided() {
        // Arrange
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<CreateContentDistributionCommandHandler>> mockLogger = new Mock<ILogger<CreateContentDistributionCommandHandler>>();
        var handler = new CreateContentDistributionCommandHandler(mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        var command = new CreateContentDistributionCommand(
            DateOnly.Parse("2024-01-01"),
            "channel",
            "method",
            [new AssetContentDistributionCommand("assetId", "fileUrl")]
        );

        // Act
        ErrorOr<ContentDistribution> result = await handler.Handle(command, CancellationToken.None);

        // Assert
        mockContentDistributionRepository.Verify(r => r.AddAsync(It.IsAny<ContentDistribution>()), Times.Once);
        mockCache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<AssetFileUrlCache>(), It.IsAny<Type>()), Times.Once);
        Assert.Equal(command.DistributionDate, result.Value.DistributionDate);
        Assert.Equal(command.DistributionChannel, result.Value.DistributionChannel);
        Assert.Equal(command.DistributionMethod, result.Value.DistributionMethod);
        Assert.Equal(command.AssetContentDistributions[0].AssetId, result.Value.AssetContentDistributions[0].AssetId.Value);
        Assert.Equal(command.AssetContentDistributions[0].FileUrl, result.Value.AssetContentDistributions[0].FileUrl);
    }

    [Fact]
    public async Task Handle_ShouldCacheFileUrl() {
        // Arrange
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<CreateContentDistributionCommandHandler>> mockLogger = new Mock<ILogger<CreateContentDistributionCommandHandler>>();
        var handler = new CreateContentDistributionCommandHandler(mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        List<AssetContentDistributionCommand> assetContentDistributionCommands = [
            new AssetContentDistributionCommand("assetId", "fileUrl"),
            new AssetContentDistributionCommand("assetId2", "fileUrl2"),
            new AssetContentDistributionCommand("assetId3", "fileUrl3")
        ];
        var command = new CreateContentDistributionCommand(
            DateOnly.Parse("2024-01-01"),
            "channel",
            "method",
            assetContentDistributionCommands
        );

        mockCache.Setup(c => c.Get(It.IsAny<string>(), It.IsAny<Type>())).Returns((AssetFileUrlCache?)null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockCache.Verify(c =>
                c.Set(It.IsAny<string>(), It.IsAny<AssetFileUrlCache>(), It.IsAny<Type>()),
            Times.Exactly(assetContentDistributionCommands.Count)
        );
    }

    [Fact]
    public async Task Handle_ShouldNotCacheFileUrl_WhenItIsAlreadyCachedAndNotOutdated() {
        // Arrange
        Mock<IContentDistributionRepository> mockContentDistributionRepository = new Mock<IContentDistributionRepository>();
        Mock<ICacheService> mockCache = new Mock<ICacheService>();
        Mock<ILogger<CreateContentDistributionCommandHandler>> mockLogger = new Mock<ILogger<CreateContentDistributionCommandHandler>>();
        var handler = new CreateContentDistributionCommandHandler(mockContentDistributionRepository.Object, mockCache.Object, mockLogger.Object);
        var command = new CreateContentDistributionCommand(
            DateOnly.Parse("2024-01-01"),
            "channel",
            "method",
            [new AssetContentDistributionCommand("assetId", "fileUrl")]
        );
        var cachedUrl = new AssetFileUrlCache("fileUrl", command.DistributionDate);

        mockCache.Setup(c => c.Get(It.IsAny<string>(), It.IsAny<Type>())).Returns(cachedUrl);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mockCache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<AssetFileUrlCache>(), It.IsAny<Type>()), Times.Never);
    }

}