using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicSystem.Decomposition
{
    public static class LinqExtensions
    {
        public static IEnumerable<(TFirst a, TOther b)> CartesianProduct<TFirst, TOther>(this IEnumerable<TFirst> first, IEnumerable<TOther> other)
        {
            return first.SelectMany(a => other.Select(o => (a, o)));
        }

        public static IEnumerable<(T a, T b)> AllPairs<T>(this IEnumerable<T> list)
        {
            return list.SelectMany((value, index) => list.Skip(index + 1), (first, second) => (first, second));
        }
    }
}