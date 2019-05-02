using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Library.CodeStructures.Behavioral
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Perform an action against
        /// </summary>
        public static void Do<S>(this IEnumerable<S> source, Action<S> action)
        {
            foreach (var entry in source)
            {
                action(entry);
            }
        }

        public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
        {
            if (ascending)
            {
                return source.OrderBy(selector);
            }
            else
            {
                return source.OrderByDescending(selector);
            }
        }
    }
}
