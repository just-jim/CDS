namespace Mock.Asset.Services;

public interface ISqsController {
    public Task Publish<TMessage>(string queueName, TMessage asset) where TMessage : Models.Asset;
}