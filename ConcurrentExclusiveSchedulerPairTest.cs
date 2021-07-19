using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyProgramme
{
    public static class ConcurrentExclusiveSchedulerPairTest
    {
        public static void Demo(IEnumerable<IEnumerable<string>> collections)
        {
            var schedulerPair = new ConcurrentExclusiveSchedulerPair(
                TaskScheduler.Default, maxConcurrencyLevel: 8);
            TaskScheduler   scheduler = schedulerPair.ConcurrentScheduler;
            ParallelOptions options   = new ParallelOptions {TaskScheduler = scheduler};
            Parallel.ForEach(collections, options, s => Console.Write(s));
        }

        public static async Task TaskRateLimiter()
        {
            var schedulerPair = new ConcurrentExclusiveSchedulerPair(
                TaskScheduler.Default, maxConcurrencyLevel: 8);

            var taskFactory = new TaskFactory(schedulerPair.ConcurrentScheduler);
            var tasks = Enumerable.Range(1, 100).Select(i => taskFactory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Console.WriteLine(i.ToString() + DateTime.Now);
            }));
            await Task.WhenAll(tasks);
        }

        public static async Task ReaderWriter()
        {
            var schedulerPair = new ConcurrentExclusiveSchedulerPair(
                TaskScheduler.Default, maxConcurrencyLevel: 8);

            var taskFactory = new TaskFactory(schedulerPair.ConcurrentScheduler);
            var single      = new TaskFactory(schedulerPair.ExclusiveScheduler);

            var readTasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                readTasks.Add(taskFactory.StartNew(() => { Read(); }));
            }

            await Task.WhenAll(readTasks);

            var writeTask = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                writeTask.Add(single.StartNew(() => Write(i.ToString())));
            }

            await Task.WhenAll(writeTask);
        }

        static string Read()
        {
            Console.WriteLine("create" + DateTime.Now);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return DateTime.Now.ToString();
        }

        static void Write(string str)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine("consume" + str);
        }
    }
}