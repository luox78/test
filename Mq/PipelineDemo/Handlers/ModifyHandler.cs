using System;
using System.Threading;
using Shared.Pipeline;

namespace PipelineDemo
{
    public class ModifyHandler : IPipelineHandler<Input, Output>
    {
        public int Order { get; } = 1;

        public Output Handle(Func<Input, CancellationToken, Output> next, Input request, CancellationToken cancellationToken)
        {
            request.Value += nameof(ModifyHandler);
            Console.WriteLine($"ModifyHandler before next, intput string :{request.Value}");

            var result = next(request, cancellationToken);

            result.Result += nameof(ModifyHandler);
            Console.WriteLine($"ModifyHandler after next, result string :{result.Result}");

            return result;
        }
    }
}