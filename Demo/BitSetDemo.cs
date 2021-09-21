using System;
using System.Collections;

namespace Demo
{
    public static class BitSetDemo
    {
        public static void New()
        {
            var bs = new BitArray(32 * 1000);
            //set index 1 mark true
            bs.Set(1, true);
            Console.WriteLine($"Bitset index 1 is marked {bs.Get(1)}");

            Console.WriteLine($"Bitset index 2 is marked {bs.Get(2)}");
        }
    }
}