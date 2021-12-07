using System;
using System.Threading;
using System.Threading.Tasks;
using Shared;

namespace Producer
{
    public class SendMessaageHandler : IProducePipelineHandler
    {
        private readonly IProducer _producer;

        public SendMessaageHandler(IProducer producer)
        {
            _producer = producer;
        }

        public int Order { get; } = 0;

        public async Task<MessageSentResult> Handle(
            Func<TransportMessage, CancellationToken, Task<MessageSentResult>> next,
            TransportMessage request, CancellationToken cancellationToken)
        {
            try
            {
                request.BodyBytes = request.Features.Get<SerializeFeature>().Serialized;

                await _producer.SendAsync(request);
                return new MessageSentResult() { Success = true };
            }
            catch (Exception e)
            {
                return new MessageSentResult() { Success = false };
            }
        }
    }
}