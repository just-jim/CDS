using Mock.Content.Distribution.Models;

namespace Mock.Content.Distribution.Services;

public interface ISqsController {
    public Task Publish<TMessage>(string queueName, TMessage asset) where TMessage : ContentDistribution;
}