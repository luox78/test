using System;
using System.Collections.Generic;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace Consumer
{
    public partial class DemoConsumer : IConsumer
    {
        public static string Exchange = "mytopic";
        public static string QueueName = "OrderWorker";

        private readonly IModel _channel;
        private readonly Dictionary<string, Type> _types = new();

        public DemoConsumer()
        {
            var factory    = new ConnectionFactory() { HostName = "localhost", Port = 50000 };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            foreach (var key in topics)
            {
                _channel.QueueBind(QueueName, Exchange, key);
            }
        }

        public void Listening(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnConsumerReceived;

            _channel.ExchangeDeclare(exchange: Exchange, type: "topic");
            _channel.QueueDeclare(QueueName);

            _channel.BasicConsume(QueueName, false, consumer);
        }

        public void Commit(object sender)
        {
            _channel.BasicAck((ulong)sender, false);
        }

        public void Reject(object sender)
        {
            _channel.BasicReject((ulong)sender, true);
        }

        public event EventHandler<TransportMessage> OnMessageReceived;
        public Type GetType(string topic)
        {
            return _types[topic];
        }

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            OnMessageReceived?.Invoke(e.DeliveryTag, new TransportMessage()
            {
                BodyBytes = e.Body.ToArray(),
                Topic     = e.RoutingKey
            });
        }
    }
}