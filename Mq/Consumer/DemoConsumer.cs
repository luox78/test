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

        public DemoConsumer(IConnectionFactory connectionFactory)
        {
            using var connection = connectionFactory.CreateConnection();
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

            _channel.BasicConsume(QueueName, false, consumer);

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                cancellationToken.WaitHandle.WaitOne(timeout);
            }
        }

        public void Commit(object sender)
        {
            _channel.BasicAck((ulong)sender, false);
        }

        public void Reject(object sender)
        {
            _channel.BasicReject((ulong)sender, true);
        }

        public event EventHandler<byte[]> OnMessageReceived;

        private void OnConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            OnMessageReceived?.Invoke(e.DeliveryTag, e.Body.ToArray());
        }
    }
}