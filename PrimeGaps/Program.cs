using PrimesGenerator;
using System;
using System.Diagnostics;

namespace PrimeGaps
{
    /// <summary>
    /// Calculates maximum distance between two consecutive primes below one trillion.
    /// </summary>
    class Program
    {
        const long MAX = 1_000_000_000_000;

        static void Main(string[] args)
        {
            var globalTimer = Stopwatch.StartNew();
            var sieve = new OptimizedSegmentedWheel235(MAX);
            long maxDistance = 0;
            long lastPrime = 0;

            Action<long> action = (p) =>
            {
                long dist = p - lastPrime;
                if (dist > maxDistance)
                {
                    Console.WriteLine($"{p} - {lastPrime} = {dist}");
                    maxDistance = dist;
                }
                lastPrime = p;
            };
            sieve.ListPrimes(action);

            Console.WriteLine($"Processed primes up to {MAX:N0} in {globalTimer.Elapsed}: Max Distance={maxDistance}");
        }

    }
}
