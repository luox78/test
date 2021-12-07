using System;
using System.Threading.Tasks;
using Shared;

namespace Consumer
{
    public class CreateOrderHandler : IMessageHandler<CreateOrderEvent>
    {
        public Task Handle(CreateOrderEvent message)
        {
            Console.WriteLine(message.OrderId);
            return Task.CompletedTask;
        }
    }
}