using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using Shared;

namespace Producer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var message = new CreateOrderEvent() {OrderId = "orderid"};
            BasicProducer.Product(message);
        }
    }
}