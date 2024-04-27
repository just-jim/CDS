namespace Mock.Order.Services;

public interface ISqsController {
    public Task Publish<TMessage>(string queueName, TMessage asset) where TMessage : Models.Order;
}