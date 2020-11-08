using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PrimesGenerator
{
    /// <summary>
    /// Un-optimized implementation of sieve of Eratosthenes with 
    /// wheel factorization with basis (2, 3, 5).
    /// https://en.wikipedia.org/wiki/Wheel_factorization
    /// </summary>
    public class Wheel235 : ISieve
    {
        private static int[] BIT_TO_INDEX = new int[] { 1, 7, 11, 13, 17, 19, 23, 29 };

        private static int[] INDEX_TO_BIT = new int[] {
            -1, 0,
            -1, -1, -1, -1, -1, 1,
            -1, -1, -1, 2,
            -1, 3,
            -1, -1, -1, 4,
            -1, 5,
            -1, -1, -1, 6,
            -1, -1, -1, -1, -1, 7,
        };

        private byte[] Data;
        public long Length { get; private set; }

        public Wheel235(long length)
        {
            Length = length;
            Data = new byte[(length + 29) / 30];
            for (long i = 0; i < Data.Length; i++) Data[i] = byte.MaxValue;

            for (long i = 7; i * i < Length; i++)
            {
                if (!IsPrime(i)) continue;
                for (long d = i * i; d < Length; d += i) ClearPrime(d);
            }
        }

        public bool IsPrime(long n)
        {
            if (n >= Length) throw new ArgumentException("Number too big");
            if (n <= 5) return n == 2 || n == 3 || n == 5;
            int bit = INDEX_TO_BIT[n % 30];
            if (bit < 0) return false;
            return (Data[n / 30] & (1 << bit)) != 0;
        }

        private void ClearPrime(long n)
        {
            int bit = INDEX_TO_BIT[n % 30];
            if (bit < 0) return;
            Data[n / 30] &= (byte)~(1 << bit);
        }

        public void ListPrimes(Action<long> callback)
        {
            callback.Invoke(2);
            callback.Invoke(3);
            callback.Invoke(5);

            for (long position = 0; position < Data.Length; position++)
            {
                for (int bit = 0; bit < 8; bit++)
                {
                    if ((Data[position] & (1 << bit)) != 0)
                    {
                        long p = position * 30 + BIT_TO_INDEX[bit];
                        if (p <= 5) continue;
                        if (p >= Length) return;
                        callback.Invoke(p);
                    }
                }
            }
        }

    }
}
