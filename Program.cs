using System;
using System.Threading.Tasks;

namespace ConcurrencyProgramme
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("start at" + DateTime.Now);

            //BlockingCollectionTest.Test();
            //BufferBlockTest.Test().GetAwaiter().GetResult();
            //ManualResetEventSlimTest.Demo();
            //await TaskCompletionSourceTest.Demo();
            //await RateLimiter.UseParallel();
            //await RateLimiter.UseSemaphoreSlim();
            //await ConcurrentExclusiveSchedulerPairTest.TaskRateLimiter();
            await ConcurrentExclusiveSchedulerPairTest.ReaderWriter();

            Console.WriteLine("end at" + DateTime.Now);
        }
    }
}