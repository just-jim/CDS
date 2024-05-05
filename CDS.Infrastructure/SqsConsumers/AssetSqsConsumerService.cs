using Amazon.SQS;
using CDS.Contracts.Commands;
using CDS.Contracts.Interfaces.Consumers;
using CDS.Contracts.Models.ExternalDomains;
using CDS.Infrastructure.SqsConsumers.Poller;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers;

public class AssetSqsConsumerService(IServiceProvider serviceProvider) : ISqsConsumerService {

    readonly ILogger<AssetSqsConsumerService> _logger = serviceProvider.GetRequiredService<ILogger<AssetSqsConsumerService>>();
    readonly IAmazonSQS _sqs = serviceProvider.GetRequiredService<IAmazonSQS>();
    readonly IConfiguration _configuration = serviceProvider.GetRequiredService<IConfiguration>();

    public Type GetMessageObjectType() {
        return typeof(AssetDomainAsset);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(_logger, _sqs);
        await poller.Polling(_configuration["AssetsQueueName"]!, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public async void HandleMessage(IMessage message) {
        var asset = (AssetDomainAsset)message;

        _logger.LogInformation($"consumed Asset with id '{asset.AssetId}'");
        var command = new CreateAssetCommand(asset.AssetId, asset.Name, asset.Description, asset.FileFormat, asset.FileSize, asset.Path);
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }
}