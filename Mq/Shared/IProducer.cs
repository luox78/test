using System.Threading.Tasks;

namespace Shared
{
    public interface IProducer
    {
        Task SendAsync(TransportMessage message);
    }
}