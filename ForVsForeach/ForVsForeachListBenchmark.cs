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
        
        private List<int> _list = null;
        
        [GlobalSetup]
        public void Setup()
        {
            _list = Enumerable.Range(0, CollectionSize).ToList();
        }

        [Benchmark]
        public int ForLoop()
        {
            var sum = 0;

            for (var i = 0; i < _list.Count; i++)
            {
                sum += _list[i];
            }
            
            return sum;
        }

        [Benchmark]
        public int ForLoopOptimized()
        {
            var sum = 0;
            
            var array = _list;
            var length = array.Count;

            for (var i = 0; i < length; i++)
            {
                sum += array[i];
            }
            
            return sum;
        }

        [Benchmark]
        public int ForeachLoop()
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