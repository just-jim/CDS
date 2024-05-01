using CDS.Application.Common.Interfaces.Models;
using Microsoft.Extensions.Hosting;

namespace CDS.Application.Common.Interfaces.Consumers;

public interface ISqsConsumerService : IHostedService {
    public Type GetMessageObjectType();
    public void HandleMessage(IMessage message);
}