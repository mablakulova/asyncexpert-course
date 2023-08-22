using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    public class FibonacciCalc
    {
        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n <= 1) return n;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (n <= 1) return n;

            var cache = new Dictionary<ulong, ulong>();
            if (cache.ContainsKey(n))
            {
                return cache[n];
            }

            cache[n] = RecursiveWithMemoization(n - 1) + RecursiveWithMemoization(n - 2);
            return cache[n];
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n <= 1) return n;
            ulong next, back1 = 1, back2 = 0;

            for (ulong i = 2; i < n; i++)
            {
                next = back1 + back2;
                back2 = back1;
                back1 = next;
            }

            return back1 + back2;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 10;
            yield return 100;
        }
    }
}
