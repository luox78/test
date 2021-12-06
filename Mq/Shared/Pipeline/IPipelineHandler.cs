using System;
using System.Threading;

namespace Shared.Pipeline
{
    public interface IPipelineHandler<TIn, TResult>
    {
        public int Order { get; }

        TResult Handle(Func<TIn, CancellationToken, TResult> next, TIn request, CancellationToken cancellationToken);
    }
}