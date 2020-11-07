using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PrimesGenerator
{
    /// <summary>
    /// Classic sieve of Sundaram without optimizations
    /// https://en.wikipedia.org/wiki/Sieve_of_Sundaram
    /// </summary>
    public class SieveOfSundaram : ISieve
    {
        private BitArray Data;
        public int Length { get; private set; }

        public SieveOfSundaram(int length)
        {
            Length = length;
            Data = new BitArray((Length + 1) / 2);
            Data.SetAll(true);

            for(int i = 1; i + i + 2 * i * i < Data.Length; i++)
            {
                for(int j = i; i + j + 2 * i * j < Data.Length; j++)
                {
                    Data[i + j + 2 * i * j] = false;
                }
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            callback.Invoke(2);

            for (int i = 1; i < Data.Length; i++)
            {
                if (Data[i])
                {
                    int p = i * 2 + 1;
                    if (p >= Length) break;
                    callback.Invoke(p);
                }
            }
        }

    }
}
