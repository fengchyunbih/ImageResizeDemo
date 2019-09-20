using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageResizer
{
    public static class Extensions
    {
        public static double Std(this IEnumerable<long> source)
        {
            var count = source.Count();
            var avg = source.Average();
            var sum = source.Sum(s => Math.Pow(s - avg, 2));
            return count > 1 ? Math.Sqrt(sum / (count - 1)) : 0;
        }
    }
}
