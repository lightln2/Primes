# Collection of various prime sieving algorithms and comparison of their performance

- simple, un-optimized implementations of Sieve of Eratosthenes, Sundaram and Atkin
- sieve of Eratosthenes with wheel factorization for base {2, 3, 5}
- optimized versions of Sieve of Eratosthenes and Sundaram
- segmented sieve of Eratosthenes
- segmented sieves of Eratosthenes with different wheel factorization bases
- optimized segmented sieve of Eratosthenes with wheel factorization for base {2, 3, 5}

## Build
on Windows, open solution with Visual Studio 2019 or run `dotnet build -c Release`

## Run
*PrimesGenerator.PerformanceTests* compares performance of sieving primes up to one billion;
*PrimeGaps* finds maximal distance between two consecutive primes not exceeding one trillion.

## Performance results:
```
SieveOfEratosthenes up to 1,000,000,000 in 00:00:12.5610926
SieveOfSundaram up to 1,000,000,000 in 00:00:18.1613135
SieveOfAtkin up to 1,000,000,000 in 00:00:11.3515272
Wheel235 up to 1,000,000,000 in 00:00:17.7559908
OptimizedSieveOfEratosthenes up to 1,000,000,000 in 00:00:06.5714213
OptimizedSieveOfSundaram up to 1,000,000,000 in 00:00:06.5137706
SegmentedSieveOfEratosthenes up to 1,000,000,000 in 00:00:08.6055209
OptimizedSegmentedSieve up to 1,000,000,000 in 00:00:07.0774830
SegmentedWheel2 up to 1,000,000,000 in 00:00:03.3645006
SegmentedWheel23 up to 1,000,000,000 in 00:00:02.6011295
SegmentedWheel235 up to 1,000,000,000 in 00:00:02.1980771
OptimizedSegmentedWheel235 up to 1,000,000,000 in 00:00:00.9265058
```
## License

- **[MIT license](https://github.com/lightln2/Primes/license.txt)**
- Copyright 2020 (c) lightln2
