using System.Threading.Tasks;

namespace Shared
{
    public interface IMessageHandler<in T> where T :class
    {
        Task Handle(T message);
    }
}