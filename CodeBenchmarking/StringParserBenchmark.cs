using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace CodeBenchmarking
{
    [MemoryDiagnoser]
    public class StringParserBenchmark
    {
        private const string Input = "100,250,75,300,125,50,175,225,125,350";

        [Benchmark]
        public void ParseWithLinq() => StringParser.ParseWithLinq(Input);

        [Benchmark]
        public void ParseWithForLoop() => StringParser.ParseWithForLoop(Input);

        [Benchmark]
        public void ParseWithSpan() => StringParser.ParseWithSpan(Input);
        
        [Benchmark]
        public void ParseWithSpanAndCache()
        {
            var arr = ArrayPool<int>.Shared.Rent(10);
            
            StringParser.ParseWithSpanAndCache(Input, ref arr);

            ArrayPool<int>.Shared.Return(arr);
        }
        
        [Benchmark]
        public void ParseWithSpanAndCustomParsing()
        {
            var arr = ArrayPool<int>.Shared.Rent(10);
            
            StringParser.ParseWithSpanAndCustomParsing(Input, ref arr);

            ArrayPool<int>.Shared.Return(arr);
        }
        
        [Benchmark]
        public void ParseWithSpanAndUncheckedParsing()
        {
            var arr = ArrayPool<int>.Shared.Rent(10);
            
            StringParser.ParseWithSpanAndUncheckedParsing(Input, ref arr);

            ArrayPool<int>.Shared.Return(arr);
        }
    }
}