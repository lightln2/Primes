using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PrimesGenerator.Tests
{
    [TestClass]
    public class SieveTests
    {
        void Test(ISieve sieve, long expectedCount, long expectedSum, long expectedHash)
        {
            long count = 0, sum = 0, hash = 0;
            Action<long> action = (p) => { count++; sum += p; hash = hash * 31 + p; };
            sieve.ListPrimes(action);
            Assert.AreEqual(expectedCount, count);
            Assert.AreEqual(expectedSum, sum);
            Assert.AreEqual(expectedHash, hash);
        }

        ISieve CreateSieve<T>(long length)
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(long) });
            if (ctor != null) return (ISieve)ctor.Invoke(new object[] { length });
            ctor = typeof(T).GetConstructor(new[] { typeof(int) });
            if(ctor != null) return (ISieve)ctor.Invoke(new object[] { (int)length });
            throw new ArgumentException($"Valid constructor not found for {typeof(T).Name}");
        }

        void Test<T>(long length, long expectedCount, long expectedSum, long expectedHash) where T : ISieve
        {
            ISieve t = CreateSieve<T>(length);
            Test(t, expectedCount, expectedSum, expectedHash);
        }

        void Test<T>() where T : ISieve
        {
            Test<T>(10, 4, 17, 62627);
            Test<T>(100, 25, 1060, 1963949867053217204);
            Test<T>(1000, 168, 76127, 8785397767903408189);
            Test<T>(10000, 1229, 5736396, 5573816336430266424);
            Test<T>(100000, 9592, 454396537, -3671880377401276645);
            Test<T>(1000000, 78498, 37550402023, -823187737887015125);
            Test<T>(10000000, 664579, 3203324994356, -6287968567270595630);
            Test<T>(100000000, 5761455, 279209790387276, 785985878218508666);
        }

        [TestMethod]
        public void Test01_SieveOfEratosthenes()
        {
            Test<SieveOfEratosthenes>();
        }

        [TestMethod]
        public void Test02_SieveOfSundaram()
        {
            Test<SieveOfSundaram>();
        }

        [TestMethod]
        public void Test03_SieveOfAtkin()
        {
            Test<SieveOfAtkin>();
        }

        [TestMethod]
        public void Test04_Wheel235()
        {
            Test<Wheel235>();
        }

        [TestMethod]
        public void Test05_OptimizedSieveOfEratosthenes()
        {
            Test<OptimizedSieveOfEratosthenes>();
        }

        [TestMethod]
        public void Test06_OptimizedSieveOfSundaram()
        {
            Test<OptimizedSieveOfSundaram>();
        }

        [TestMethod]
        public void Test07_SegmentedSieveOfEratosthenes()
        {
            Test<SegmentedSieveOfEratosthenes>();
        }

        [TestMethod]
        public void Test08_OptimizedSegmentedSieve()
        {
            Test<OptimizedSegmentedSieve>();
        }

        [TestMethod]
        public void Test09_SegmentedWheel2()
        {
            Test<SegmentedWheel2>();
        }

        [TestMethod]
        public void Test10_SegmentedWheel23()
        {
            Test<SegmentedWheel23>();
        }

        [TestMethod]
        public void Test11_SegmentedWheel235()
        {
            Test<SegmentedWheel235>();
        }

        [TestMethod]
        public void Test12_OptimizedSegmentedWheel235()
        {
            Test<OptimizedSegmentedWheel235>();
        }

    }
}
