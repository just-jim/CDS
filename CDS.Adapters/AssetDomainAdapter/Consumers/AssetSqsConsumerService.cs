using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CDS.Adapters.AssetDomainAdapter.SqsModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CDS.Adapters.AssetDomainAdapter.Consumers;

public class AssetSqsConsumerService(ILogger<AssetSqsConsumerService> logger, IAmazonSQS sqs) : IHostedService {
    readonly ILogger _logger = logger;

    const string QueueName = "assets";
    readonly List<string> _messageAttributeNames = ["All"];

    public async Task StartAsync(CancellationToken ct) {
        var queueUrl = await sqs.GetQueueUrlAsync(QueueName, ct);
        var receiveRequest = new ReceiveMessageRequest {
            QueueUrl = queueUrl.QueueUrl,
            MessageAttributeNames = _messageAttributeNames,
            AttributeNames = _messageAttributeNames
        };
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        _logger.LogInformation("Asset sqs consumer initiated");
        while(!ct.IsCancellationRequested) {
            var messageResponse = await sqs.ReceiveMessageAsync(receiveRequest, ct);
            if (messageResponse.HttpStatusCode != HttpStatusCode.OK) {
                _logger.LogWarning("Not healthy status code while consuming from the the assets queue");
                continue;
            }

            foreach (var message in messageResponse.Messages) {
                try {
                    var asset = JsonSerializer.Deserialize<AssetDomainAsset>(message.Body, jsonSerializerOptions);
                    _logger.LogInformation($"Asset {asset?.AssetId} consumed");
                    // TODO pass the asset to a service in order to create an asset in the database
                }
                catch (JsonException ex) {
                    _logger.LogWarning("Consumed malformed asset from sqs");
                    // Here we could forward the malformed message to a DLQ
                }

                await sqs.DeleteMessageAsync(queueUrl.QueueUrl, message.ReceiptHandle, ct);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}