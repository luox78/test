using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Consumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            BasicConsumer.Start();
        }
    }
}