using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamBuilders
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Check if elements in two collections are the same.
        /// </summary>
        public static bool ContentsMatch<T>(this IEnumerable<T> firstSequence, IEnumerable<T> secondSequence)
        {
            var firstArray = firstSequence as T[] ?? firstSequence.ToArray();
            var secondArray = secondSequence as T[] ?? secondSequence.ToArray();

            if (firstArray.IsNullOrEmpty()
                && secondArray.IsNullOrEmpty())
                return true;

            if (firstArray.IsNullOrEmpty()
                || secondArray.IsNullOrEmpty()
                || firstArray.Length != secondArray.Length)
                return false;

            return firstArray.All(x1 => secondArray.Contains(x1));
        }

        /// <summary>
        /// Performs an action on each element in the sequence.
        /// </summary>
        /// <param name="action">The action to perform on each element.</param>    
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            var sequenceArray = sequence as T[] ?? sequence.ToArray();

            foreach (var item in sequenceArray) { action(item); }

            return sequenceArray;
        }

        /// <summary>
        /// Performs a function on each element in the sequence.
        /// </summary>
        public static IEnumerable<T> ForEach<T, R>(this IEnumerable<T> sequence, Func<T, R> func)
        {
            var sequenceArray = sequence as T[] ?? sequence.ToArray();

            foreach (T element in sequenceArray) func(element);

            return sequenceArray;
        }

        /// <summary>
        /// Get a random element from <paramref name="sequence"/>.
        /// </summary>
        public static T GetRandom<T>(this IEnumerable<T> sequence)
        {
            var arraySequence = sequence as T[] ?? sequence.ToArray();

            return arraySequence.ToArray()[UnityEngine.Random.Range(0, arraySequence.Count())];
        }

        /// <returns>
        /// Returns -1 if none found.
        /// </returns>
        public static int IndexOfItem<T>(this IEnumerable<T> sequence, T item)
        {
            if (sequence == null)
                return -1;

            var index = 0;
            foreach (var i in sequence)
            {
                if (Equals(i, item)) return index;
                ++index;
            }

            return -1;
        }

        /// <summary>
        /// Determines whether a collection is null or has no elements
        /// without having to enumerate the entire collection to get a count.
        ///
        /// Uses LINQ's Any() method to determine if the collection is empty,
        /// so there is some GC overhead.
        /// </summary>
        /// <param name="sequence">List to evaluate</param>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> sequence)
        {
            return sequence == null || !sequence.Any();
        }

        /// <summary>
        /// Removes a random item from the list, returning that item.
        /// Sampling without replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="removedElement">The element removed from sequence.</param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveRandom<T>(this IEnumerable<T> sequence, out T removedElement)
        {
            var newSequence = sequence.ToList();
            removedElement = GetRandom(newSequence);
            newSequence.Remove(removedElement);

            return newSequence;
        }

        /// <summary>
        /// Replace a value in sequence with a new value.
        /// </summary>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> sequence, T oldValue, T newValue)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var comparer = EqualityComparer<T>.Default;

            foreach (var item in sequence)
            {
                yield return
                    comparer.Equals(item, oldValue)
                        ? newValue
                        : item;
            }
        }

        /// <summary>
        /// Based on the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new();

            var buffer = sequence.ToList();
            for (int i = buffer.Count; i > 1; i--)
            {
                byte[] box = new byte[1];

                do provider.GetBytes(box);
                while (!(box[0] < i * (byte.MaxValue / i)));

                int k = (box[0] % i);
                i--;

                yield return buffer[i];
                buffer[k] = buffer[i];
            }
        }
    }
}