using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyProgramme
{
    public class RateLimiter
    {
        private static IEnumerable<int> _test = Enumerable.Range(1, 10);

        public static async Task UseParallel()
        {
            await Task.WhenAll(_test.AsParallel().WithDegreeOfParallelism(2).Select(async i =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                Console.WriteLine(DateTime.Now + "this is " + i);
                return i * 2;
            }).ToList());
        }

        public static async Task UseSemaphoreSlim()
        {
            var sem = new SemaphoreSlim(2);
            var tasks = _test.Select(t => Task.Run(() => Fetch(sem)));
            await Task.WhenAll(tasks);
        }

        static async Task Fetch(SemaphoreSlim sem)
        {
            await sem.WaitAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine(DateTime.Now);

            sem.Release();
        }
    }
}