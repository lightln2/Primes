using System;
using System.Collections.Generic;
using System.Text;

namespace PrimesGenerator
{
    public interface ISieve
    {
        void ListPrimes(Action<long> callback);
    }
}
