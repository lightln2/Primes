using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PrimesGenerator
{
    /// <summary>
    /// Classic sieve of Atkin without optimizations
    /// https://en.wikipedia.org/wiki/Sieve_of_Atkin
    /// </summary>
    public class SieveOfAtkin : ISieve
    {
        private BitArray Data;
        public int Length { get; private set; }

        public SieveOfAtkin(int length)
        {
            Length = length;
            Data = new BitArray(Length);
            Data.SetAll(false);

            SieveQuadForm1(Data);
            SieveQuadForm2(Data);
            SieveQuadForm3(Data);

            for (int p = 7; p * p <= length; p++)
            {
                if (Data[p])
                {
                    for (int i = p * p; i < Length; i += p * p)
                    {
                        Data[i] = false;
                    }
                }
            }

        }

        private void SieveQuadForm1(BitArray data)
        {
            for (int x = 1; 4 * x * x < data.Length; x++)
            {
                for (int y = 1; 4 * x * x + y * y < data.Length; y += 2)
                {
                    int n = 4 * x * x + y * y;
                    int rem = n % 60;
                    if (rem == 1 || rem == 13 || rem == 17 || rem == 29 || rem == 37 || rem == 41 || rem == 49 || rem == 53)
                    {
                        data[n] = !data[n];
                    }
                }
            }
        }

        private void SieveQuadForm2(BitArray data)
        {
            for (int x = 1; 3 * x * x < data.Length; x += 2)
            {
                for (int y = 2; 3 * x * x + y * y < data.Length; y += 2)
                {
                    int n = 3 * x * x + y * y;
                    int rem = n % 60;
                    if (rem == 7 || rem == 19 || rem == 31 || rem == 43)
                    {
                        data[n] = !data[n];
                    }
                }
            }
        }

        private void SieveQuadForm3(BitArray data)
        {
            for (int x = 1; 2 * x * x < data.Length; x++)
            {
                for (int y = x - 1; y > 0; y -= 2)
                {
                    int n = 3 * x * x - y * y;
                    if (n >= data.Length) continue;
                    int rem = n % 60;
                    if (rem == 11 || rem == 23 || rem == 47 || rem == 59)
                    {
                        data[n] = !data[n];
                    }
                }
            }
        }


        public void ListPrimes(Action<long> callback)
        {
            callback.Invoke(2);
            callback.Invoke(3);
            callback.Invoke(5);
            for (int i = 7; i < Length; i++)
            {
                if (Data[i])
                {
                    callback.Invoke(i);
                }
            }
        }

    }
}
