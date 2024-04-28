using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mock.Content.Distribution.Models;

namespace Mock.Content.Distribution.Services;

public class AwsSqsController(IConfiguration configuration) : ISqsController {
    readonly AmazonSQSClient _sqsClient = new AmazonSQSClient(
        new BasicAWSCredentials("ignore", "ignore"),
        new AmazonSQSConfig{ ServiceURL = configuration["LocalStackHost"] }
    );

    public async Task Publish<TMessage>(string queueName, TMessage contentDistribution) 
        where TMessage : ContentDistribution
    {
        var queueUrl = await _sqsClient.GetQueueUrlAsync(queueName);
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(contentDistribution),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "contentDistribution", 
                    new MessageAttributeValue {
                        StringValue = "contentDistribution",
                        DataType = "String"
                    }
                }
            }
        };
        await _sqsClient.SendMessageAsync(request);
    }
}