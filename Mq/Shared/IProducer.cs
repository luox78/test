using System.Threading.Tasks;

namespace Shared
{
    public interface IProducer
    {
        Task SendAsync(byte[] message);
    }
}