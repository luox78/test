using System;
using System.Threading;
using Shared.Pipeline;

namespace PipelineDemo
{
    public class LogHandler : IPipelineHandler<Input, Output>
    {
        public int Order { get; } = 2;

        public Output Handle(Func<Input, CancellationToken, Output> next, Input request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"LogHandler before next, intput string :{request.Value}");

            var result = next(request, cancellationToken);

            Console.WriteLine($"LogHandler after next, result string :{result.Result}");

            return result;
        }
    }
}