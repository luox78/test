using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Shared;
using Shared.Pipeline;

namespace Consumer.PipelineHandlers
{
    public class ConsumeMessageUseMultiChannelHandler : IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>
    {
        private readonly IDispatcher _dispatcher;
        private readonly IConsumer _consumer;
        private readonly Channel<TransportMessage>[] _channels;

        public ConsumeMessageUseMultiChannelHandler(IDispatcher dispatcher, IConsumer consumer)
        {
            _dispatcher = dispatcher;
            _consumer = consumer;

            _channels = new Channel<TransportMessage>[4];
            for (int i = 0; i < 4; i++)
            {
                _channels[i] = Channel.CreateUnbounded<TransportMessage>();
            }
            for (int i = 0; i < 4; i++)
            {
                var index = i;
                Task.Factory.StartNew(_ => Consume(index), null, TaskCreationOptions.LongRunning);
            }
        }

        public int Order { get; } = 0;

        public async Task<MessageConsumedResult> Handle(
            Func<TransportMessage, CancellationToken, Task<MessageConsumedResult>> next, TransportMessage request,
            CancellationToken cancellationToken)
        {
            //1. random随机写入channel
            var rand = new Random().Next(0, 4);
            _channels[rand].Writer.TryWrite(request);

            /*//2. 根据key计算哈希值，将同一个key的写入到同一个channel
            var messageType =_consumer.GetType(ts.Topic);
            var message     = JsonSerializer.Deserialize(request.BodyBytes, messageType);
            var key = (message as IMessage).Key;
            var hash = key.GetHashCode();
            var index = hash % 4;
            _channels[index].Writer.TryWrite(request);*/

            return new MessageConsumedResult();
        }

        async Task Consume(int index)
        {
            var channel = _channels[index];

            while (await channel.Reader.WaitToReadAsync())
            {
                while (channel.Reader.TryRead(out var ts))
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