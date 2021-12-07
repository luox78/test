using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consumer.AutoCollectAndRouteHandler;
using Consumer.PipelineHandlers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Shared;
using Shared.Pipeline;

namespace Consumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            //BasicConsumer.Start();

            var services = new ServiceCollection();

            services.AddSingleton<IConsumer, DemoConsumer>();
            services.AddSingleton<IDispatcher, SimpleDispatcher>();
            services.AddSingleton<Pipeline<TransportMessage, Task<MessageConsumedResult>>>();
            //services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>, ConsumeMessageHandler>(); 
            //services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>, ConsumeMessageUseChannelHandler>();
            //services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>, ConsumeMessageUseMultiChannelHandler>();
            services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageConsumedResult>>, ConsumeMessageUseMultiThreadHandler>();

            services.AddSingleton<IMessageHandler<CreateOrderEvent>, CreateOrderHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var consumePipeline = serviceProvider.GetService<Pipeline<TransportMessage, Task<MessageConsumedResult>>>();
            var consumer        = serviceProvider.GetService<IConsumer>();

            consumer.Listening(TimeSpan.FromSeconds(5), CancellationToken.None);
            consumer.Subscribe(consumer.FetchTopics);

            var consumeFunc = consumePipeline.CreateHandler((_, _) => null);
            consumer.OnMessageReceived += (sender, message) =>
            {
                var result = consumeFunc(message, CancellationToken.None).GetAwaiter().GetResult();
                consumer.Commit(sender);
            };

            Console.ReadLine();
        }
    }
}