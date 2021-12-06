using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Shared
{
    public interface IConsumer
    {
        string[] FetchTopics { get; }

        void Subscribe(IEnumerable<string> topics);

        void Listening(TimeSpan timeout, CancellationToken cancellationToken);

        void Commit([NotNull] object sender);

        void Reject(object sender);

        event EventHandler<byte[]> OnMessageReceived;
    }
}