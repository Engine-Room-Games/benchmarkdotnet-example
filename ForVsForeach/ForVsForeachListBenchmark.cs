using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;

namespace ForVsForeach
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByParams)]
    public class ForVsForeachListBenchmark
    {
        [Params(10_000, 100_000, 1_000_000, 10_000_000)]
        public int CollectionSize = 0;
        
        private List<int> _array = null;

        [GlobalSetup]
        public void Setup()
        {
            _array = Enumerable.Range(0, CollectionSize).ToList();
        }

        [Benchmark]
        public int ForLoop()
        {
            var sum = 0;

            for (int i = 0; i < _array.Count; i++)
            {
                sum += _array[i];
            }

            return sum;
        }

        [Benchmark]
        public int ForLoopOptimized()
        {
            var sum = 0;

            var array = _array;
            var length = array.Count;

            for (int i = 0; i < length; i++)
            {
                sum += array[i];
            }

            return sum;
        }

        [Benchmark]
        public int ForeachLoop()
        {
            var sum = 0;

            foreach (var num in _array)
            {
                sum += num;
            }

            return sum;
        }
    }
}