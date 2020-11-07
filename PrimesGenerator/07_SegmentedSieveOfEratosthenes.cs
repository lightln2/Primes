using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesGenerator
{
    /// <summary>
    /// Divides sieving range into smaller intervals and sieves each interval individually
    /// https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes#Segmented_sieve
    /// </summary>
    public class SegmentedSieveOfEratosthenes : ISieve
    {
        private long Length;
        private int FirstChunkLength;
        private long[] FirstPrimes;

        public SegmentedSieveOfEratosthenes(long length)
        {
            Length = length;
            FirstChunkLength = (int)Math.Sqrt(length) + 1;
            SieveOfEratosthenes sieve = new SieveOfEratosthenes(FirstChunkLength);
            List<long> firstPrimes = new List<long>();
            sieve.ListPrimes(firstPrimes.Add);
            FirstPrimes = firstPrimes.ToArray();
        }

        private void SieveSegment(BitArray segmentData, long segmentStart, long segmentEnd)
        {
            segmentData.SetAll(true);

            foreach (long p in FirstPrimes)
            {
                long first = p * p;
                if (first < segmentStart)
                {
                    first += (segmentStart - first + p - 1) / p * p;
                }

                for (long i = first; i < segmentEnd; i += p)
                {
                    segmentData[(int)(i - segmentStart)] = false;
                }
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            foreach (long p in FirstPrimes) callback.Invoke(p);

            int segmentLength = FirstChunkLength;
            BitArray segmentData = new BitArray(segmentLength);
            long segmentStart = FirstChunkLength;
            long segmentEnd = Math.Min(segmentStart + segmentLength, Length);
            while (segmentStart < Length)
            {
                SieveSegment(segmentData, segmentStart, segmentEnd);
                for (long i = segmentStart; i < segmentEnd; i++)
                {
                    if (segmentData[(int)(i - segmentStart)]) callback.Invoke(i);
                }
                segmentStart = segmentEnd;
                segmentEnd = Math.Min(segmentStart + segmentLength, Length);
            }
        }

    }
}
