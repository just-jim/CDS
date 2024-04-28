using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Mock.Asset.Services;

public class AwsSqsController(IConfiguration configuration) : ISqsController {
    readonly AmazonSQSClient _sqsClient = new AmazonSQSClient(
        new BasicAWSCredentials("ignore", "ignore"),
        new AmazonSQSConfig{ ServiceURL = configuration["LocalStackHost"] }
    );

    public async Task Publish<TMessage>(string queueName, TMessage asset) 
        where TMessage : Models.Asset
    {
        var queueUrl = await _sqsClient.GetQueueUrlAsync(queueName);
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(asset),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "asset", 
                    new MessageAttributeValue {
                        StringValue = "asset",
                        DataType = "String"
                    }
                }
            }
        };
        await _sqsClient.SendMessageAsync(request);
    }
}