using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Shared;

namespace Consumer.AutoCollectAndRouteHandler
{
    public class MessageProcessor
    {
        public static string Exchange = "mytopic";
        public static string QueueName = "OrderWorker";
        
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _model;

        public MessageProcessor(IServiceProvider serviceProvider, IConnectionFactory connectionFactory)
        {
            _serviceProvider = serviceProvider;

            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            foreach (var key in CollectAllRoutekey())
            {
                channel.QueueBind(queue: QueueName,
                    exchange: Exchange,
                    routingKey: key);
            }
        }

        public Task DoHandler<T>(string routekey, T message) where T:class
        {
            var handler = _serviceProvider.GetService<IMessageHandler<T>>();
            return handler.Handle(message);
        }

        static List<string> CollectAllRoutekey()
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var result = new List<string>();
            foreach (var type in allTypes)
            {
                if (typeof(IMessageHandler<>).IsAssignableFrom(type))
                {
                    var messageType = type.GenericTypeArguments[0];
                    result.Add(messageType.ToString());
                }
            }

            return result.Distinct().ToList();
        }
    }
}