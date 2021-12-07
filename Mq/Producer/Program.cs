using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Shared;
using Shared.Pipeline;

namespace Producer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var message = new CreateOrderEvent() { OrderId = "orderid" };
            BasicProducer.Product(message);

            var services = new ServiceCollection();

            services.AddSingleton<IProducer, DemoProducer>();
            services.AddSingleton<Pipeline<TransportMessage, Task<MessageSentResult>>>();
            services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageSentResult>>, SendMessaageHandler>();
            services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageSentResult>>, SerializeBodyHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var sendPipeline = serviceProvider.GetService<Pipeline<TransportMessage, Task<MessageSentResult>>>();
            var sendFunc     = sendPipeline.CreateHandler((_, _) => null);

            while (true)
            {
                Console.WriteLine("输入订单号，发送mq");

                var orderId = Console.ReadLine();
                var result = await sendFunc.Invoke(new TransportMessage()
                {
                    Message = new CreateOrderEvent() { OrderId = orderId, Key = orderId },
                    Topic   = typeof(CreateOrderEvent).FullName
                }, CancellationToken.None);

                Console.WriteLine(result.Success);

            }
        }
    }
}