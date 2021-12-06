using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Pipeline;

namespace Producer
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProducer(this IServiceCollection services)
        {
            services.AddSingleton<Pipeline<TransportMessage, Task<MessageSentResult>>>();
            services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageSentResult>>, SendMessaageHandler>();
            services.AddSingleton<IPipelineHandler<TransportMessage, Task<MessageSentResult>>, SerializeBodyHandler>();
            return services;
        }
    }
}