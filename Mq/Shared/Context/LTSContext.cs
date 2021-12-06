using System.Threading;

namespace Shared.Context
{
    //local thread storage
    public class LTSContext
    {
        public int IntValue { get; set; }

        public string StringValue { get; set; }
        //....


        public static AsyncLocal<LTSContext> Current = new();
    }
}