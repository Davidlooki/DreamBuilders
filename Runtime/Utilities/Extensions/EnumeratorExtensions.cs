using System.Collections.Generic;

namespace DreamBuilders
{
    public static class EnumeratorExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> sequence)
        {
            while (sequence.MoveNext()) { yield return sequence.Current; }
        }
    }
}