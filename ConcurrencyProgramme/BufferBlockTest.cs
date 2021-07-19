using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyProgramme
{
    public static class BufferBlockTest
    {
        public static async Task Test()
        {
            BufferBlock<int> queue = new BufferBlock<int>();

            // 生产者代码
            await queue.SendAsync(7);
            await queue.SendAsync(13);
            queue.Complete();

            // 单个消费者时的代码
            while (await queue.OutputAvailableAsync())
                Console.WriteLine(await queue.ReceiveAsync());

            // 多个消费者时的代码
            while (true)
            {
                int item;
                try
                {
                    item = await queue.ReceiveAsync();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                Console.WriteLine(item);
            }
        }
    }
}