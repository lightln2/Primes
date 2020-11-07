using System;
using System.Diagnostics;

namespace PrimesGenerator.PerformanceTests
{
    class Program
    {
        static void AssertEquals(long expected, long actual)
        {
            if (expected != actual) throw new Exception($"Expected {expected} but was {actual}");
        }

        static void TestSieve<T>(long length, long expectedCount, long expectedSum, long expectedHash)
        {
            var sw = Stopwatch.StartNew();
            ISieve sieve = CreateSieve<T>(length);
            long count = 0, sum = 0, hash = 0;
            Action<long> action = (p) => { count++; sum += p; hash = hash * 31 + p; };
            sieve.ListPrimes(action);
            AssertEquals(expectedCount, count);
            AssertEquals(expectedSum, sum);
            AssertEquals(expectedHash, hash);
            Console.WriteLine($"{typeof(T).Name} up to {length:N0} in {sw.Elapsed}");
        }

        static ISieve CreateSieve<T>(long length)
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(long) });
            if (ctor != null) return (ISieve)ctor.Invoke(new object[] { length });
            ctor = typeof(T).GetConstructor(new[] { typeof(int) });
            if (ctor != null) return (ISieve)ctor.Invoke(new object[] { (int)length });
            throw new ArgumentException($"Valid constructor not found for {typeof(T).Name}");
        }

        static void TestSieve_1G<T>() =>
            TestSieve<T>(1_000_000_000, 50847534, 24739512092254535, 1480810120364005255);

        static void TestSieve_10G<T>() =>
            TestSieve<T>(10_000_000_000, 455052511, 2220822432581729238, 3238719281018177996);

        static void TestSieve_20G<T>() =>
            TestSieve<T>(20_000_000_000, 882206716, 8617752113620426559, -3075438841046943771);

        static void Main(string[] args)
        {   
            TestSieve_1G<SieveOfEratosthenes>();
            TestSieve_1G<SieveOfSundaram>();
            TestSieve_1G<SieveOfAtkin>();
            TestSieve_1G<Wheel235>();
            TestSieve_1G<OptimizedSieveOfEratosthenes>();
            TestSieve_1G<OptimizedSieveOfSundaram>();
            TestSieve_1G<SegmentedSieveOfEratosthenes>();
            TestSieve_1G<OptimizedSegmentedSieve>();
            TestSieve_1G<SegmentedWheel2>();
            TestSieve_1G<SegmentedWheel23>();
            TestSieve_1G<SegmentedWheel235>();
            TestSieve_1G<OptimizedSegmentedWheel235>();
            
        }
    }
}
