using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Consumer.AutoCollectAndRouteHandler
{
    public class SimpleDispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task ExecuteAsync<T>(T message) where T : IMessage
        {
            var handler = _serviceProvider.GetService<IMessageHandler<T>>() ?? throw new Exception("");
            return handler.Handle(message);
        }

        public Task Execute(object message)
        {
            var messageType = message.GetType();
            var m = this.GetType().GetMethod("ExecuteAsync");
            var result = this.GetType()
                .GetMethod("ExecuteAsync")
                .MakeGenericMethod(messageType)
                .Invoke(this, new[] { message }) as Task;
            return result;
        }
    }
}