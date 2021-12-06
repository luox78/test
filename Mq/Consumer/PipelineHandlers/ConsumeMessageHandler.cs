using System;
using System.Threading;
using System.Threading.Tasks;
using Shared;
using Shared.Pipeline;

namespace Consumer.PipelineHandlers
{
    public class ConsumeMessageHandler : IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>
    {
        private readonly IConsumer _consumer;

        public ConsumeMessageHandler(IConsumer consumer)
        {
            _consumer = consumer;
        }

        public int Order { get; } = 0;

        public Task<MessageConsumedResult> Handle(
            Func<TransportMessage, CancellationToken, Task<MessageConsumedResult>> next, TransportMessage request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}