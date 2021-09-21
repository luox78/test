using System.Collections;
using System.Collections.Generic;

namespace Demo
{
    public class BloomFilter
    {
        class InternalHash
        {
            private readonly int _cap;
            private readonly int _seed;

            public InternalHash(int cap, int seed)
            {
                _cap  = cap;
                _seed = seed;
            }

            public int Hash(string str)
            {
                return 1;
            }
        }

        private readonly BitArray _bitArray;
        private readonly List<InternalHash> _funcs;

        public BloomFilter(int cap, int funs)
        {
            _funcs = new List<InternalHash>();
            for (int i = 0; i < funs; i++)
            {
                _funcs.Add(new InternalHash(cap, i));
            }

            _bitArray = new BitArray(cap);
        }

        public void Add(string str)
        {
            foreach (var internalHash in _funcs)
            {
                _bitArray.Set(internalHash.Hash(str), true);
            }
        }

        public bool Exist(string str)
        {
            var result = true;
            foreach (var internalHash in _funcs)
            {
                if (!_bitArray.Get(internalHash.Hash(str)))
                {
                    result = false;
                }
            }

            return result;
        }
    }
}