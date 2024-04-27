using Microsoft.Extensions.Hosting;

namespace CDS.Adapters.Interfaces;

public interface ISqsConsumerService : IHostedService {
    public Type GetMessageObjectType();
    public void HandleMessage(IMessage message);
}