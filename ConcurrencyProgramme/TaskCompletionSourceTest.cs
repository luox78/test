using System;
using System.Threading.Tasks;

namespace ConcurrencyProgramme
{
    public class TaskCompletionSourceTest
    {
        private readonly TaskCompletionSource<object> initialized =
            new TaskCompletionSource<object>();

        private int value1;
        private int value2;

        public async Task<int> WaitForInitializationAsync()
        {
            await initialized.Task;
            return value1 + value2;
        }

        public void Initialize()
        {
            value1 = 13;
            value2 = 17;
            initialized.TrySetResult(null);
        }

        public static async Task Demo()
        {
            var test = new TaskCompletionSourceTest();
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                test.Initialize();
            });

            Console.WriteLine(await test.WaitForInitializationAsync());
        }
    }
}