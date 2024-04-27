using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Mock.Asset.Services;

public class SqsController(IAmazonSQS sqs) {

    public async Task Publish<TMessage>(string queueName, TMessage asset) 
        where TMessage : Models.Asset
    {
        var queueUrl = await sqs.GetQueueUrlAsync(queueName);
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
        await sqs.SendMessageAsync(request);
    }
}