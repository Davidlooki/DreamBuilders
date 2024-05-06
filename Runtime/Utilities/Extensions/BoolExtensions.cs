using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamBuilders
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Check if <paramref name="item"/> is among <paramref name="options"/>.
        /// <example>
        /// <code>
        /// var fruits = new[] {"apple", "orange", "grape"};
        /// "apple".IsOneOf(fruits);
        /// </code>
        /// results is true.
        /// </example>
        /// </summary>
        /// <returns>bool</returns>
        public static bool IsOneOf<T>(this T item, params T[] options) =>
            Array.Exists(options, x => x.Equals(item));
    }
}