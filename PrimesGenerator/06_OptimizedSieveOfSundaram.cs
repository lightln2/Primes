using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PrimesGenerator
{
    /// <summary>
    /// Trivial optimization of sieve of Sundaram that only processes pairs {i, j}
    /// where 2 * i + 1 is prime
    /// </summary>
    public class OptimizedSieveOfSundaram : ISieve
    {
        private BitArray Data;
        public int Length { get; private set; }

        public OptimizedSieveOfSundaram(int length)
        {
            Length = length;
            Data = new BitArray((Length + 1) / 2);
            Data.SetAll(true);

            for (int i = 1; i + i + 2 * i * i < Data.Length; i++)
            {
                // this check is not part of original algorithm,
                // but it does not make sense not to do it
                if (!Data[i]) continue;

                for (int j = i; i + j + 2 * i * j < Data.Length; j++)
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
