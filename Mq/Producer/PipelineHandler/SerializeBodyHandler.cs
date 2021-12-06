using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace Producer
{
    public class SerializeBodyHandler : IProducePipelineHandler
    {
        public int Order { get; } = 1;

        public Task<MessageSentResult> Handle(Func<TransportMessage, CancellationToken, Task<MessageSentResult>> next,
            TransportMessage request, CancellationToken cancellationToken)
        {
            var str = JsonSerializer.Serialize(request.Message);
            request.Features.Set(new SerializeFeature()
            {
                Serialized = Encoding.UTF8.GetBytes(str)
            });
            return next(request, cancellationToken);
        }
    }
}