using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Shared;

namespace Producer
{
    public class DemoProducer : IProducer
    {
        private readonly IModel _channel;

        public static string Exchange = "mytopic";

        public DemoProducer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 50000 };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare(exchange: Exchange, type: "topic");
        }

        public Task SendAsync(TransportMessage message)
        {
            var props = _channel.CreateBasicProperties();
            props.DeliveryMode = 2;

            _channel.BasicPublish(exchange: Exchange,
                routingKey: message.Topic,
                basicProperties: props,
                body: message.BodyBytes);

            _channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }
    }
}