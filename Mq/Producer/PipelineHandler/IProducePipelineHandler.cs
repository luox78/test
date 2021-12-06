using System.Threading.Tasks;
using Shared;
using Shared.Pipeline;

namespace Producer
{
    public interface IProducePipelineHandler : IPipelineHandler<TransportMessage, Task<MessageSentResult>>
    {
    }
}