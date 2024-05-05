using Microsoft.Extensions.Hosting;

namespace CDS.Contracts.Interfaces.Consumers;

public interface ISqsConsumerService : IHostedService {
    public Type GetMessageObjectType();
    public void HandleMessage(IMessage message);
}