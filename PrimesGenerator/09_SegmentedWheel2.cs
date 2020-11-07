using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesGenerator
{
    public class SegmentedWheel2 : ISieve
    {
        const int BUFFER_LENGTH = 64 * 1024;
        private long Length;
        private long[] FirstPrimes;
        private long[] PrimeMultiples;

        public SegmentedWheel2(long length)
        {
            Length = length;
            int firstChunkLength = (int)Math.Sqrt(length) + 1;
            SieveOfEratosthenes sieve = new SieveOfEratosthenes(firstChunkLength);
            List<long> firstPrimes = new List<long>();
            sieve.ListPrimes(firstPrimes.Add);
            FirstPrimes = firstPrimes.Skip(1).ToArray();
            PrimeMultiples = FirstPrimes.Select(p => (p * p - 1) / 2).ToArray();
        }

        private void SieveSegment(BitArray segmentData, long segmentStart, long segmentEnd)
        {
            segmentData.SetAll(true);
            long segmentLength = segmentEnd - segmentStart;
            for(int i = 0; i < PrimeMultiples.Length; i++)
            {
                long current = PrimeMultiples[i] - segmentStart;
                long prime = FirstPrimes[i];
                if (current >= segmentLength) continue;

                while(current < segmentLength)
                {
                    segmentData[(int)current] = false;
                    current += prime;
                }
                PrimeMultiples[i] = segmentStart + current;
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            callback.Invoke(2);
            BitArray segmentData = new BitArray(BUFFER_LENGTH);
            long max = Length / 2;
            long segmentStart = 1;
            long segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            while (segmentStart < max)
            {
                SieveSegment(segmentData, segmentStart, segmentEnd);
                for (int i = 0; i < segmentEnd - segmentStart; i++)
                {
                    if (segmentData[i])
                    {
                        callback.Invoke((segmentStart + i) * 2 + 1);
                    }
                }
                segmentStart = segmentEnd;
                segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            }
        }

    }
}
