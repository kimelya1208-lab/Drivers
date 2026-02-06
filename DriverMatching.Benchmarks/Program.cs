using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DriverMatching;

namespace DriverMatching.Benchmarks
{
    [MemoryDiagnoser]
    public class DriverMatchingBenchmark
    {
        private List<Driver> drivers;
        private Order order;
        private int k;

        [Params(100, 1_000, 10_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            drivers = new List<Driver>();
            var rnd = new Random(42);

            for (int i = 0; i < N; i++)
            {
                int x = rnd.Next(-1000, 1000);
                int y = rnd.Next(-1000, 1000);
                drivers.Add(new Driver(i + 1, x, y));
            }

            order = new Order(2, 2);
            k = 5;
        }

        [Benchmark(Baseline = true)]
        public List<Driver> Naive()
        {
            return DriverMatcher.FindNearestDriversNaive(drivers, order, k);
        }

        [Benchmark]
        public List<Driver> TopK()
        {
            return DriverMatcher.FindNearestDriversTopK(drivers, order, k);
        }

        [Benchmark]
        public List<Driver> Heap()
        {
            return DriverMatcher.FindNearestDriversHeap(drivers, order, k);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<DriverMatchingBenchmark>();
        }
    }
}