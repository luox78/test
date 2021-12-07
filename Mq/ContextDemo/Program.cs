using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ContextDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            LTSContext.Current.Value = new LTSContext() { List = new List<string>() { "from main" } };

            await T1();

            await Task.WhenAll(T2(), T3());
        }

        static async Task T1()
        {
            await Task.Yield();
            LTSContext.Current.Value.List.Add("from T1");
            Console.WriteLine($"T1, thread: {Thread.CurrentThread.ManagedThreadId}, Context: {GetContextList()}");
        }

        static async Task T2()
        {
            await Task.Yield();
            Console.WriteLine($"T2, thread: {Thread.CurrentThread.ManagedThreadId}, Context: {GetContextList()}");
        }

        static async Task T3()
        {
            await Task.Yield();
            Console.WriteLine($"T3, thread: {Thread.CurrentThread.ManagedThreadId}, Context: {GetContextList()}");
        }

        static string GetContextList()
        {
            return string.Join('|', LTSContext.Current.Value.List);
        }

        class LTSContext
        {
            public List<string> List { get; set; }

            public static AsyncLocal<LTSContext> Current = new();
        }
    }
}
