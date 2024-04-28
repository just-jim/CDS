using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Mock.Order.Services;

public class AwsSqsController(IConfiguration configuration) : ISqsController {
    readonly AmazonSQSClient _sqsClient = new AmazonSQSClient(
        new BasicAWSCredentials("ignore", "ignore"),
        new AmazonSQSConfig { ServiceURL = configuration.GetConnectionString("LocalStack") }
    );

    public async Task Publish<TMessage>(string queueName, TMessage order)
        where TMessage : Models.Order {
        var queueUrl = await _sqsClient.GetQueueUrlAsync(queueName);
        var request = new SendMessageRequest {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(order),
            MessageAttributes = new Dictionary<string, MessageAttributeValue> {
                {
                    "order",
                    new MessageAttributeValue {
                        StringValue = "order",
                        DataType = "String"
                    }
                }
            }
        };
        await _sqsClient.SendMessageAsync(request);
    }
}