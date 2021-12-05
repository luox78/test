using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Producer
{
    public static class BasicProducer
    {
        public static string Exchange = "mytopic";
        
        public static void Product<T>(T message) where T:class
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 50000};
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: Exchange, type: "topic");

                var routekey = typeof(T).ToString();
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                
                channel.BasicPublish(exchange: Exchange,
                    routingKey: routekey,
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", routekey, message);
            }
        }
    }
}