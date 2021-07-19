using System;
using System.Collections.Concurrent;

namespace ConcurrencyProgramme
{
    public static class BlockingCollectionTest
    {
        public static void Test()
        {
            var a = new BlockingCollection<string>();
            a.Add("nihao");
            a.CompleteAdding();
            try
            {
                a.Add("more");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            foreach (var str in a.GetConsumingEnumerable())
            {
                Console.WriteLine(str);
            }

            var bQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
            var bStack = new BlockingCollection<string>(new ConcurrentStack<string>());
        }
    }
}