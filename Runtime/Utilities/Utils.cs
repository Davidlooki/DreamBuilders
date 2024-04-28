using System;

namespace DreamBuilders
{
    public static class Utils
    {
        public static bool IsOneOf<T>(this T item, params T[] options) =>
            Array.Exists(options, x => x.Equals(item));
    }
}