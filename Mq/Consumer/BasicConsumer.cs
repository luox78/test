using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace Consumer
{
    public static class BasicConsumer
    {
        public static string Exchange = "mytopic";
        public static string QueueName = "OrderWorker";
        
        public static void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 50000};
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: Exchange, type: "topic");
                channel.QueueDeclare(QueueName);

                //指定该queue绑定哪些消息routekey和哪个exchange
                channel.QueueBind(queue: QueueName,
                    exchange: Exchange,
                    routingKey: typeof(CreateOrderEvent).ToString());

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                        routingKey,
                        message);
                };
                channel.BasicConsume(queue: QueueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}