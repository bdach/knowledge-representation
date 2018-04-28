using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicSystem.Decomposition
{
    /// <summary>
    /// Extension functions for commonly used linq idioms. Should make the code more readable.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Function for generating the cartesian product of two enumerables.
        /// </summary>
        /// <typeparam name="TFirst">Type of elements of the first collection</typeparam>
        /// <typeparam name="TOther">Type of elements of the second collection</typeparam>
        /// <param name="first">First collection</param>
        /// <param name="other">Second collection</param>
        /// <returns>IEnumerable of named tuples which represent the element of the cartesian product</returns>
        public static IEnumerable<(TFirst a, TOther b)> CartesianProduct<TFirst, TOther>(this IEnumerable<TFirst> first, IEnumerable<TOther> other)
        {
            return first.SelectMany(a => other.Select(o => (a, o)));
        }

        /// <summary>
        /// Function for generating all unique unordered pairs from an enumarable. 
        /// Should NOT return two pairs with the same elements but in a different order.
        /// </summary>
        /// <typeparam name="T">Type of elements of the enumerable</typeparam>
        /// <param name="list">Enumerable from which the pairs are generated</param>
        /// <returns>IEnumerable of named tuples which represent the generated pairs</returns>
        public static IEnumerable<(T a, T b)> AllPairs<T>(this IEnumerable<T> list)
        {
            return list.SelectMany((value, index) => list.Skip(index + 1), (first, second) => (first, second));
        }
    }
}