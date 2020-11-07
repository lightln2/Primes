using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesGenerator
{
    /// <summary>
    /// This optimization stores every 30 sieved numbers in one byte,
    /// which avoids calculation of byte/bit offset to update the bit array
    /// </summary>
    public class OptimizedSegmentedWheel235 : ISieve
    {
        const int BUFFER_LENGTH = 200 * 1024;
        const int WHEEL = 30;
        const int WHEEL_PRIMES_COUNT = 3;

        private static long[] WheelRemainders = { 1, 7, 11, 13, 17, 19, 23, 29 };
        private static long[] SkipPrimes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };
        private static byte[] Masks = { 1, 2, 4, 8, 16, 32, 64, 128 };
        private static int[][] OffsetsPerByte;

        private long Length;
        private long[] FirstPrimes;
        private long[][] PrimeMultiples;

        static OptimizedSegmentedWheel235()
        {
            OffsetsPerByte = new int[256][];
            List<int> offsets = new List<int>();
            for (int b = 0; b < 256; b++)
            {
                offsets.Clear();
                for (int i = 0; i < WheelRemainders.Length; i++)
                {
                    if ((b & Masks[i]) != 0) offsets.Add((int)WheelRemainders[i]);
                }
                OffsetsPerByte[b] = offsets.ToArray();
            }
        }

        public OptimizedSegmentedWheel235(long length)
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

        private void SieveSegment(byte[] segmentData, long segmentStart, long segmentEnd)
        {
            for (int i = 0; i < segmentData.Length; i++) segmentData[i] = 255;
            long segmentLength = segmentEnd - segmentStart;

            for (int i = 0; i < WheelRemainders.Length; i++)
            {
                byte mask = (byte)~Masks[i];
                for (int j = 0; j < PrimeMultiples[i].Length; j++)
                {
                    long current = PrimeMultiples[i][j] - segmentStart;
                    if (current >= segmentLength) continue;
                    long prime = FirstPrimes[j];

                    while (current < segmentLength)
                    {
                        segmentData[current] &= mask;
                        current += prime;
                    }

                    PrimeMultiples[i][j] = segmentStart + current;
                }
            }
        }

        public void ListPrimes(Action<long> callback)
        {
            foreach (long prime in SkipPrimes) if (prime < Length) callback.Invoke(prime);

            long max = (Length + WHEEL - 1) / WHEEL;
            byte[] segmentData = new byte[BUFFER_LENGTH];
            long segmentStart = 1;
            long segmentEnd = Math.Min(segmentStart + BUFFER_LENGTH, max);
            while (segmentStart < max)
            {
                SieveSegment(segmentData, segmentStart, segmentEnd);
                for (int i = 0; i < segmentData.Length; i++)
                {
                    long offset = (segmentStart + i) * WHEEL;
                    byte data = segmentData[i];
                    int[] offsets = OffsetsPerByte[data];
                    for (int j = 0; j < offsets.Length; j++)
                    {
                        long p = offset + offsets[j];
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
