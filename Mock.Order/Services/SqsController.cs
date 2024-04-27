using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Mock.Order.Services;

public class SqsController(IAmazonSQS sqs) {

    public async Task Publish<TMessage>(string queueName, TMessage order) 
        where TMessage : Models.Order
    {
        var queueUrl = await sqs.GetQueueUrlAsync(queueName);
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl.QueueUrl,
            MessageBody = JsonSerializer.Serialize(order),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "order", 
                    new MessageAttributeValue {
                        StringValue = "order",
                        DataType = "String"
                    }
                }
            }
        };
        await sqs.SendMessageAsync(request);
    }
}