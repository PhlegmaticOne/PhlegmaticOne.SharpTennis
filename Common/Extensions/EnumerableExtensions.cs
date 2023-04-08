using System.Collections.Generic;
using System.Linq;

namespace PhlegmaticOne.SharpTennis.Game.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectAtOddIndexes<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Where((x, i) => i % 2 == 0);
        }

        public static IEnumerable<T> SelectAtIndexes<T>(this IEnumerable<T> enumerable, params int[] indexes)
        {
            var all = indexes.ToList();
            return enumerable.Where((x, i) => all.Contains(i));
        }

        public static IEnumerable<T> SelectAtIndexesInOrder<T>(this IEnumerable<T> enumerable, params int[] indexes)
        {
            var all = enumerable.ToList();
            return indexes.Select(index => all[index]).ToList();
        }
    }
}
