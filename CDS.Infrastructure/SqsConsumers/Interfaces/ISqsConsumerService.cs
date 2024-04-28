using Microsoft.Extensions.Hosting;

namespace CDS.Infrastructure.SqsConsumers.Interfaces;

public interface ISqsConsumerService : IHostedService {
    public Type GetMessageObjectType();
    public void HandleMessage(IMessage message);
}