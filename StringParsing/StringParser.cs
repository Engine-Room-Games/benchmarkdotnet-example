using System.Runtime.CompilerServices;

namespace StringParsing
{
    public static class StringParser
    {
        public static int[] ParseWithLinq(string input)
        {
            return input.Split(',').Select(int.Parse).ToArray();
        }
        
        public static int[] ParseWithForLoop(string input)
        {
            var parts = input.Split(',');
            var arr = new int[parts.Length];

            for (var i = 0; i < parts.Length; i++)
            {
                arr[i] = int.Parse(parts[i]);
            }

            return arr;
        }
        
        // Improving performance by utilizing the stack memory
        public static int[] ParseWithSpan(string input)
        {
            var inputSpan = input.AsSpan();
            var count = inputSpan.Count(',') + 1;
            
            Span<int> buffer = stackalloc int[count];

            var index = 0;
            var start = 0;

            for (var i = 0; i <= inputSpan.Length; i++)
            {
                if (i != inputSpan.Length && inputSpan[i] != ',')
                {
                    continue;
                }
                
                var slice = inputSpan.Slice(start, i - start);
                int.TryParse(slice, out buffer[index]);
                index++;
                start = i + 1;
            }
            
            return buffer.ToArray();
        }
        
        // Adding the ability to work with cached array - this should reduce allocations in a long run
        public static void ParseWithSpanAndCache(string input, ref int[] arr)
        {
            var inputSpan = input.AsSpan();
            var count = inputSpan.Count(',') + 1;
            
            Span<int> buffer = stackalloc int[count];

            var index = 0;
            var start = 0;

            for (var i = 0; i <= inputSpan.Length; i++)
            {
                if (i != inputSpan.Length && inputSpan[i] != ',')
                {
                    continue;
                }
                
                var slice = inputSpan.Slice(start, i - start);
                int.TryParse(slice, out buffer[index]);
                index++;
                start = i + 1;
            }
            
            buffer.CopyTo(arr);
        }
        
        // Using custom parsing for the span - should improve the performance
        public static void ParseWithSpanAndCustomParsing(string input, ref int[] arr)
        {
            var inputSpan = input.AsSpan();
            var count = inputSpan.Count(',') + 1;

            Span<int> buffer = stackalloc int[count];

            var index = 0;
            var start = 0;

            for (var i = 0; i <= inputSpan.Length; i++)
            {
                if (i != inputSpan.Length && inputSpan[i] != ',')
                {
                    continue;
                }

                var slice = inputSpan.Slice(start, i - start);
                TryParseSpan(slice, out buffer[index]);
                index++;
                start = i + 1;
            }

            buffer.CopyTo(arr);
        }

        // Claude can take almost all the credit for this custom parsing method
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParseSpan(ReadOnlySpan<char> s, out int value)
        {
            value = 0;
            if (s.IsEmpty) return false;

            var i = 0;
            var sign = 1;

            var c0 = s[0];
            if (c0 is '+' or '-')
            {
                if (s.Length == 1) return false;
                sign = c0 == '-' ? -1 : 1;
                i = 1;
            }

            // Accumulate negatively to safely represent Int32.MinValue.
            var result = 0;                           // negative accumulator
            const int minQuot = int.MinValue / 10;    // -214748364
            const int minRem  = int.MinValue % 10;    // -8

            for (; i < s.Length; i++)
            {
                var d = s[i] - '0';
                if ((uint)d > 9u) return false;

                // Check: result*10 - d >= int.MinValue, without doing it if it would overflow.
                if (result < minQuot || (result == minQuot && -d < minRem))
                    return false;

                result = result * 10 - d;             // stays in negative range
            }

            if (sign == 1)
            {
                // For positive numbers we must not exceed Int32.MaxValue.
                if (result == int.MinValue) return false; // would be 2147483648
                value = -result;
            }
            else
            {
                value = result;
            }
            return true;
        }
        
        // Another implementation of custom int parsing - this should be even faster
        public static void ParseWithSpanAndUncheckedParsing(string input, ref int[] arr)
        {
            var inputSpan = input.AsSpan();
            var count = inputSpan.Count(',') + 1;

            Span<int> buffer = stackalloc int[count];

            var index = 0;
            var start = 0;

            for (var i = 0; i <= inputSpan.Length; i++)
            {
                if (i != inputSpan.Length && inputSpan[i] != ',')
                {
                    continue;
                }

                var slice = inputSpan.Slice(start, i - start);
                ParseUnchecked(slice, ref buffer[index]);
                index++;
                start = i + 1;
            }

            buffer.CopyTo(arr);
        }
        
        // This one is super dangerous. It won't work with negative values,
        // it doesn't have any error checks, etc.
        // It should be really fast, but should be used with caution (probably shouldn't be
        // used at all and exists purely for academic purpose)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseUnchecked(ReadOnlySpan<char> str, ref int number)
        {
            foreach (var c in str)
            {
                number = number * 10 + (c - '0');
            }
        }
    }
}