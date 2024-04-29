using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CDS.Infrastructure.SqsConsumers.Interfaces;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers.Poller;

public class SqsPoller(ILogger logger, IAmazonSQS sqs) {
    readonly List<string> _messageAttributeNames = ["All"];

    public delegate void MessageHandling(IMessage message);

    public async Task Polling(string queue, Type objectType, MessageHandling callback, CancellationToken ct) {

        var queueUrl = await sqs.GetQueueUrlAsync(queue, ct);
        var receiveRequest = new ReceiveMessageRequest {
            QueueUrl = queueUrl.QueueUrl,
            MessageAttributeNames = _messageAttributeNames,
            AttributeNames = _messageAttributeNames
        };
        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        logger.LogInformation($"{queue} sqs consumer initiated");
        while(!ct.IsCancellationRequested) {
            var messageResponse = await sqs.ReceiveMessageAsync(receiveRequest, ct);
            if (messageResponse.HttpStatusCode != HttpStatusCode.OK) {
                logger.LogWarning($"Not healthy status code while consuming from the the {queue} queue");
                continue;
            }

            foreach (var message in messageResponse.Messages) {
                try {
                    var messageObject = (IMessage?)JsonSerializer.Deserialize(message.Body, objectType, jsonSerializerOptions);
                    logger.LogInformation($"{queue} {messageObject?.Id()} consumed");
                    callback(messageObject!);
                }
                catch (JsonException e) {
                    logger.LogError($"Consumed malformed message from {queue} sqs queue",e);
                    // Here we could forward the malformed message to a DLQ
                }

                await sqs.DeleteMessageAsync(queueUrl.QueueUrl, message.ReceiptHandle, ct);
            }
            await Task.Delay(500, ct);
        }
    }
}