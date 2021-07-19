using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyProgramme
{
    public class ManualResetEventSlimTest
    {
        private readonly ManualResetEventSlim initialized =
            new ManualResetEventSlim();

        private int value;

        public int WaitForInitialization()
        {
            initialized.Wait();
            return value;
        }

        public void InitializeFromAnotherThread()
        {
            value = 13;
            initialized.Set();
        }

        public static void Demo()
        {
            var test = new ManualResetEventSlimTest();
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                test.InitializeFromAnotherThread();
            });
            Console.WriteLine(test.WaitForInitialization());
        }
    }
}