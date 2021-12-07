using System.Threading.Tasks;
using Shared;

namespace Consumer
{
    public interface IDispatcher
    {
        public Task ExecuteAsync<T>(T message) where T : IMessage;

        public Task Execute(object message);
    }
}