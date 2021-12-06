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
            var message = new CreateOrderEvent() {OrderId = "orderid"};
            BasicProducer.Product(message);

            var services = new ServiceCollection();
            services.AddProducer();

            var serviceProvider = services.BuildServiceProvider();

            var sendPipeline = serviceProvider.GetService<Pipeline<TransportMessage, Task<MessageSentResult>>>();
            var sendFunc = sendPipeline.CreateHandler((_, _) => null);

            var result = await sendFunc.Invoke(new TransportMessage()
            {
                Message = new CreateOrderEvent() { OrderId = "OrderId" }
            }, CancellationToken.None);
            Console.WriteLine(result.Success);
        }
    }
}