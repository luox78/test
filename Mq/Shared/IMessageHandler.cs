using System.Threading.Tasks;

namespace Shared
{
    public interface IMessageHandler<in T> where T : IMessage
    {
        Task Handle(T message);
    }
}