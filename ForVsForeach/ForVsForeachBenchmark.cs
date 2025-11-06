using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;

namespace ForVsForeach
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByParams)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ForVsForeachBenchmark
    {
        [Params(10_000, 100_000, 1_000_000, 10_000_000)] 
        public int CollectionSize = 0;

        private int[] _array = null;
        private List<int> _list = null;

        [GlobalSetup]
        public void Setup()
        {
            _array = Enumerable.Range(0, CollectionSize).ToArray();
            _list = _array.ToList();
        }

        [Benchmark(Description = "Array: For")]
        public int ArrayForLoop()
        {
            var sum = 0;
            var array = _array;
            
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            
            return sum;
        }

        [Benchmark(Description = "Array: Foreach")]
        public int ArrayForeachLoop()
        {
            var sum = 0;
            
            foreach (var num in _array)
            {
                sum += num;
            }
            
            return sum;
        }

        [Benchmark(Description = "List: For")]
        public int ListForLoop()
        {
            var sum = 0;
            
            var list = _list;
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }
            
            return sum;
        }

        [Benchmark(Description = "List: Foreach")]
        public int ListForeachLoop()
        {
            var sum = 0;
            
            foreach (var num in _list)
            {
                sum += num;
            }
            
            return sum;
        }
    }
}