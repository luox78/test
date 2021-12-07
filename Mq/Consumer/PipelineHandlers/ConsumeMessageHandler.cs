using System;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Shared;
using Shared.Pipeline;

namespace Consumer.PipelineHandlers
{
    public class ConsumeMessageHandler : IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>
    {
        private readonly IDispatcher _dispatcher;
        private readonly IConsumer _consumer;

        public ConsumeMessageHandler(IDispatcher dispatcher, IConsumer consumer)
        {
            _dispatcher = dispatcher;
            _consumer   = consumer;
        }

        public int Order { get; } = 0;

        public async Task<MessageConsumedResult> Handle(
            Func<TransportMessage, CancellationToken, Task<MessageConsumedResult>> next, TransportMessage request,
            CancellationToken cancellationToken)
        {
            var messageType = _consumer.GetType(request.Topic);
            var message     = JsonSerializer.Deserialize(request.BodyBytes, messageType);
            await _dispatcher.Execute(message);

            return new MessageConsumedResult();
        }
    }
}