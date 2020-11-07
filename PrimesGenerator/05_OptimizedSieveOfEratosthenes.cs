using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PrimesGenerator
{
    /// <summary>
    /// Simple optimization of sieve of Eratosthenes that only sieves odd numbers
    /// </summary>
    public class OptimizedSieveOfEratosthenes : ISieve
    {
        private BitArray Data;
        public int Length { get; private set; }

        public OptimizedSieveOfEratosthenes(int length)
        {
            Length = length;
            Data = new BitArray(Length / 2 + 1);
            Data.SetAll(true);

            int maxFactor = (int)Math.Sqrt(Length);

            for (int p = 3; p <= maxFactor; p += 2)
            {
                if (Data[p / 2])
                {
                    for (int i = p * p; i < Length; i += 2 * p)
                    {
                        Data[i / 2] = false;
                    }
                }
            }

        }

        public void ListPrimes(Action<long> callback)
        {
            callback.Invoke(2);

            for (int i = 1; i < Length / 2; i++)
            {
                if (Data[i])
                {
                    long p = i * 2 + 1;
                    if (p >= Length) break;
                    callback.Invoke(p);
                }
            }
        }

    }
}
