using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesGenerator
{
    public class SegmentedWheel235 : ISieve
    {
        const int BUFFER_LENGTH = 128 * 1024;
        const int WHEEL = 30;
        const int WHEEL_PRIMES_COUNT = 3;
        private long[] WheelRemainders = { 1, 7, 11, 13, 17, 19, 23, 29 };
        private long[] SkipPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };

        private long Length;
        private long[] FirstPrimes;
        private long[][] PrimeMultiples;

        public SegmentedWheel235(long length)
        {
            Length = length;
            int firstChunkLength = (int)Math.Sqrt(length) + 1;
            SieveOfEratosthenes sieve = new SieveOfEratosthenes(firstChunkLength);
            List<long> firstPrimes = new List<long>();
            sieve.ListPrimes(firstPrimes.Add);
            FirstPrimes = firstPrimes.Skip(WHEEL_PRIMES_COUNT).ToArray();
            PrimeMultiples = new long[WheelRemainders.Length][];
            for(int i = 0; i < WheelRemainders.Length; i++)
            {
                PrimeMultiples[i] = new long[FirstPrimes.Length];
                for(int j = 0; j < FirstPrimes.Length; j++)
                {
                    long prime = FirstPrimes[j];
                    long val = prime * prime;
                    while (val % WHEEL != WheelRemainders[i]) val += 2 * prime;
                    PrimeMultiples[i][j] = (val - WheelRemainders[i]) / WHEEL;
                }
            }
        }

        private void SieveSegment(BitArray[] segmentDatas, long segmentStart, long segmentEnd)
        {
            for(int i = 0; i < segmentDatas.Length; i++)
            {
                BitArray segmentData = segmentDatas[i];
                segmentData.SetAll(true);
                long segmentLength = segmentEnd - segmentStart;
                for (int j = 0; j < PrimeMultiples[i].Length; j++)
                {
                    long current = PrimeMultiples[i][j] - segmentStart;
                    long prime = FirstPrimes[j];
                    if (current >= segmentLength) continue;

                    while (current < segmentLength)
                    {
                        segmentData[(int)current] = false;
                        current += prime;
                    }
                    PrimeMultiples[i][j] = segmentStart + current;
                }
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            foreach (long prime in SkipPrimes) if(prime < Length) callback.Invoke(prime);

            BitArray[] segmentDatas = new BitArray[WheelRemainders.Length];
            for(int i = 0; i < segmentDatas.Length; i++)
            {
                segmentDatas[i] = new BitArray(BUFFER_LENGTH);
            }
            long max = (Length + WHEEL - 1) / WHEEL;
            long segmentStart = 1;
            long segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            while (segmentStart < max)
            {
                SieveSegment(segmentDatas, segmentStart, segmentEnd);
                for (int i = 0; i < segmentEnd - segmentStart; i++)
                {
                    long offset = (segmentStart + i) * WHEEL;
                    for (int j = 0; j < segmentDatas.Length; j++)
                    {
                        if (segmentDatas[j][i])
                        {
                            long p = offset + WheelRemainders[j];
                            if (p >= Length) break;
                            callback.Invoke(p);
                        }
                    }
                }
                segmentStart = segmentEnd;
                segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            }
        }

    }
}
