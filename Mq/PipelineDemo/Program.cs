using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Shared.Pipeline;

namespace PipelineDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<Pipeline<Input, Output>>();
            services.AddSingleton<IPipelineHandler<Input, Output>, LogHandler>();
            services.AddSingleton<IPipelineHandler<Input, Output>, ModifyHandler>();
            services.AddSingleton<IPipelineHandler<Input, Output>, EndPointHandler>();

            var serviceProvider = services.BuildServiceProvider();
            var pipeline = serviceProvider.GetService<Pipeline<Input, Output>>();
            
            var func = pipeline.CreateHandler((i, ctx) => null);

            var result = func.Invoke(new Input()
            {
                Value = "Hello pipeline"
            }, CancellationToken.None);
            Console.WriteLine(result.Result);
        }
    }
}