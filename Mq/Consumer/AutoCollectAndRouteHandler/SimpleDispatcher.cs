using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Consumer.AutoCollectAndRouteHandler
{
    public class SimpleDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute<T>(T message) where T : IMessage
        {
            var handler = _serviceProvider.GetService<IMessageHandler<T>>() ?? throw new Exception("");
            return handler.Handle(message);
        }
    }
}