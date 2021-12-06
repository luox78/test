using System;
using System.Threading;
using Shared.Pipeline;

namespace PipelineDemo
{
    public class EndPointHandler : IPipelineHandler<Input, Output>
    {
        public int Order { get; } = 0;

        public Output Handle(Func<Input, CancellationToken, Output> next, Input request, CancellationToken cancellationToken)
        {
            return new Output()
            {
                Result = $"this is a result from EndPointHandler, request is {request.Value}"
            };
        }
    }
}