using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Shared;
using Shared.Pipeline;

namespace Consumer.PipelineHandlers
{
    public class ConsumeMessageUseMultiThreadHandler : IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>
    {
        private readonly IDispatcher _dispatcher;
        private readonly IConsumer _consumer;
        private readonly Channel<TransportMessage> _channel;

        public ConsumeMessageUseMultiThreadHandler(IDispatcher dispatcher, IConsumer consumer)
        {
            _dispatcher = dispatcher;
            _consumer = consumer;

            _channel = Channel.CreateUnbounded<TransportMessage>();

            //四个线程同时消费
            for (int i = 0; i < 4; i++)
            {
                Task.Factory.StartNew(_ => Consume(), null, TaskCreationOptions.LongRunning);
            }
        }

        public int Order { get; } = 0;

        public async Task<MessageConsumedResult> Handle(
            Func<TransportMessage, CancellationToken, Task<MessageConsumedResult>> next, TransportMessage request,
            CancellationToken cancellationToken)
        {
            _channel.Writer.TryWrite(request);
            return new MessageConsumedResult();
        }

        async Task Consume()
        {
            while (await _channel.Reader.WaitToReadAsync())
            {
                while (_channel.Reader.TryRead(out var ts))
                {
                    try
                    {
                        var messageType = _consumer.GetType(ts.Topic);
                        var message     = JsonSerializer.Deserialize(ts.BodyBytes, messageType);
                        await _dispatcher.Execute(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}