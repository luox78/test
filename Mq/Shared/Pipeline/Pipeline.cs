using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Shared.Pipeline
{
    public class Pipeline<TIn, TResult>
    {
        private readonly IPipelineHandler<TIn, TResult>[] _handlers;

        public Pipeline(IEnumerable<IPipelineHandler<TIn, TResult>> handlers)
        {
            _handlers = handlers.ToArray();
        }

        public Func<TIn, CancellationToken, TResult> CreateHandler(Func<TIn, CancellationToken, TResult> origin)
        {
            var result = origin;
            foreach (var handler in _handlers.OrderBy(h => h.Order))
            {
                var tmp = result;
                result = (r, t) => handler.Handle(tmp, r, t);
            }

            return result;
        }
    }
}