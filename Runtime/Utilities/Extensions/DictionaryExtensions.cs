using System.Collections.Generic;
using System.Linq;

namespace DreamBuilders
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Is Keys in dictionary the same as some sequence.
        /// </summary>
        public static bool ContentsMatchKeys<T1, T2>(this IDictionary<T1, T2> dictionary, IEnumerable<T1> sequence)
        {
            var sequenceArray = sequence as T1[] ?? sequence.ToArray();

            if (dictionary.IsNullOrEmpty() && sequenceArray.IsNullOrEmpty()) return true;
            if (dictionary.IsNullOrEmpty() || sequenceArray.IsNullOrEmpty()) return false;

            return dictionary.Keys.ContentsMatch(sequenceArray);
        }

        /// <summary>
        /// Is Values in dictionary the same as some sequence.
        /// </summary>
        public static bool ContentsMatchValues<T1, T2>(this IDictionary<T1, T2> dictionary, IEnumerable<T2> sequence)
        {
            var sequenceArray = sequence as T2[] ?? sequence.ToArray();

            if (dictionary.IsNullOrEmpty() && sequenceArray.IsNullOrEmpty()) return true;
            if (dictionary.IsNullOrEmpty() || sequenceArray.IsNullOrEmpty()) return false;

            return dictionary.Values.ContentsMatch(sequenceArray);
        }
    }
}