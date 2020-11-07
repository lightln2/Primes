using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PrimesGenerator
{
    /// <summary>
    /// Classic sieve of Eratosphenes without optimizations
    /// https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
    /// </summary>
    public class SieveOfEratosthenes : ISieve
    {
        private BitArray Data;
        public int Length => Data.Length;

        public SieveOfEratosthenes(int length)
        {
            Data = new BitArray(length);
            Data.SetAll(true);

            for (int p = 2; p * p < length; p++)
            {
                if (Data[p])
                {
                    for (int i = p * p; i < Length; i += p)
                    {
                        Data[i] = false;
                    }
                }
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            for (int i = 2; i < Length; i++)
            {
                if (Data[i]) callback.Invoke(i);
            }
        }

    }
}
