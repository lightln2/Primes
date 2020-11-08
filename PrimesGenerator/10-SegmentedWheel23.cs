using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesGenerator
{
    public class SegmentedWheel23 : ISieve
    {
        const int BUFFER_LENGTH = 128 * 1024;

        private long Length;
        private long[] FirstPrimes;
        private long[] PrimeMultiples_6kPlus1;
        private long[] PrimeMultiples_6kPlus5;

        public SegmentedWheel23(long length)
        {
            Length = length;
            int firstChunkLength = (int)Math.Sqrt(length) + 1;
            SieveOfEratosthenes sieve = new SieveOfEratosthenes(firstChunkLength);
            List<long> firstPrimes = new List<long>();
            sieve.ListPrimes(firstPrimes.Add);
            FirstPrimes = firstPrimes.Skip(2).ToArray();
            PrimeMultiples_6kPlus1 = new long[FirstPrimes.Length];
            PrimeMultiples_6kPlus5 = new long[FirstPrimes.Length];
            for(int j = 0; j < FirstPrimes.Length; j++)
            {
                long prime = FirstPrimes[j];
                long val1 = prime * prime;
                while (val1 % 6 != 1) val1 += 2 * prime;
                PrimeMultiples_6kPlus1[j] = (val1 - 1) / 6;
                long val2 = prime * prime;
                while (val2 % 6 != 5) val2 += 2 * prime;
                PrimeMultiples_6kPlus5[j] = (val2 - 5) / 6;
            }
        }

        private void SieveSegment(BitArray segmentData_6kPlus1, BitArray segmentData_6kPlus5, long segmentStart, long segmentEnd)
        {
            segmentData_6kPlus1.SetAll(true);
            segmentData_6kPlus5.SetAll(true);
            long segmentLength = segmentEnd - segmentStart;
            for (int j = 0; j < FirstPrimes.Length; j++)
            {
                long prime = FirstPrimes[j];

                long val1 = PrimeMultiples_6kPlus1[j] - segmentStart;
                while (val1 < segmentLength)
                {
                    segmentData_6kPlus1[(int)val1] = false;
                    val1 += prime;
                }
                PrimeMultiples_6kPlus1[j] = val1 + segmentStart;

                long val2 = PrimeMultiples_6kPlus5[j] - segmentStart;
                while (val2 < segmentLength)
                {
                    segmentData_6kPlus5[(int)val2] = false;
                    val2 += prime;
                }
                PrimeMultiples_6kPlus5[j] = val2 + segmentStart;
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            callback.Invoke(2);
            callback.Invoke(3);
            callback.Invoke(5);
            BitArray segmentData_6kPlus1 = new BitArray(BUFFER_LENGTH);
            BitArray segmentData_6kPlus5 = new BitArray(BUFFER_LENGTH);
            long max = (Length + 5) / 6;
            long segmentStart = 1;
            long segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            while (segmentStart < max)
            {
                SieveSegment(segmentData_6kPlus1, segmentData_6kPlus5, segmentStart, segmentEnd);
                for (int i = 0; i < segmentEnd - segmentStart; i++)
                {
                    if(segmentData_6kPlus1[i])
                    {
                        long p = (segmentStart + i) * 6 + 1;
                        if (p >= Length) break;
                        callback.Invoke(p);
                    }
                    if (segmentData_6kPlus5[i])
                    {
                        long p = (segmentStart + i) * 6 + 5;
                        if (p >= Length) break;
                        callback.Invoke(p);

                    }
                }
                segmentStart = segmentEnd;
                segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            }
        }

    }
}
