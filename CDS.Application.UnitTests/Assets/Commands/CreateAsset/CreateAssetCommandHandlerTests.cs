using CDS.Application.Assets.Commands.CreateAsset;
using CDS.Application.Common.Interfaces.Clients;
using CDS.Application.Common.Interfaces.Database;
using CDS.Application.Common.Models;
using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.Entities;
using CDS.Domain.AssetAggregate.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace CDS.Application.UnitTests.Assets.Commands.CreateAsset;

public class CreateAssetCommandHandlerTests {
    [Fact]
    public async Task Handle_ShouldUpdateAsset_WhenAssetExists() {
        // Arrange
        var mockAssetRepository = new Mock<IAssetRepository>();
        var mockBriefingQueryService = new Mock<IQueryService>();
        var mockLogger = new Mock<ILogger<CreateAssetCommandHandler>>();
        var handler = new CreateAssetCommandHandler(mockAssetRepository.Object, mockBriefingQueryService.Object, mockLogger.Object);
        var command = new CreateAssetCommand("existingId", "param2", "param3", "param4", "param5", "param6");
        var existingAsset = Asset.Create(
            AssetId.Create("existingId"),
            "param2",
            "param3",
            "param4",
            "param5",
            "param6",
            Briefing.Create("briefingParam", DateOnly.Parse("2022-01-01")
            )
        );

        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(true);
        mockAssetRepository.Setup(r => r.GetByIdAsync(It.IsAny<AssetId>())).ReturnsAsync(existingAsset);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        mockAssetRepository.Verify(r => r.AddAsync(It.IsAny<Asset>()), Times.Never);
        mockAssetRepository.Verify(r => r.UpdateAsync(existingAsset), Times.Once);
        Assert.Equal(existingAsset, result.Value);
    }

    [Fact]
    public async Task Handle_ShouldCreateAsset_WhenAssetDoesNotExist() {
        // Arrange
        var mockAssetRepository = new Mock<IAssetRepository>();
        var mockBriefingQueryService = new Mock<IQueryService>();
        var mockLogger = new Mock<ILogger<CreateAssetCommandHandler>>();
        var handler = new CreateAssetCommandHandler(mockAssetRepository.Object, mockBriefingQueryService.Object, mockLogger.Object);
        var command = new CreateAssetCommand("newId", "param2", "param3", "param4", "param5", "param6");

        var briefingDomainBriefing = new BriefingDomainBriefing("param2", "param3", "briefingParam", "2022-01-01");
        mockAssetRepository.Setup(r => r.ExistsAsync(It.IsAny<AssetId>())).ReturnsAsync(false);
        mockBriefingQueryService.Setup(s => s.FetchDataAsync(It.IsAny<string>())).ReturnsAsync(briefingDomainBriefing);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        mockAssetRepository.Verify(r => r.AddAsync(It.IsAny<Asset>()), Times.Once);
        Assert.Equal(command.AssetId, result.Value.Id.Value);
        Assert.Equal(command.Name, result.Value.Name);
        Assert.Equal(command.Description, result.Value.Description);
        Assert.Equal(command.FileFormat, result.Value.FileFormat);
        Assert.Equal(command.FileSize, result.Value.FileSize);
        Assert.Equal(command.Path, result.Value.Path);
        Assert.Equal(briefingDomainBriefing.CreatedBy, result.Value.Briefing.CreatedBy);
        Assert.Equal(DateOnly.Parse(briefingDomainBriefing.CreatedDate), result.Value.Briefing.CreatedDate);
    }
}