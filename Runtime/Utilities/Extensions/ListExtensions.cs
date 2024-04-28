using System;
using System.Collections.Generic;
using System.Linq;
using static System.Byte;

namespace DreamBuilders
{
    public static class ListExtensions
    {
        /// <summary>
        /// Determines whether a collection is null or has no elements
        /// without having to enumerate the entire collection to get a count.
        ///
        /// Uses LINQ's Any() method to determine if the collection is empty,
        /// so there is some GC overhead.
        /// </summary>
        /// <param name="list">List to evaluate</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list) {
            return list == null || !list.Any();
        }

        /// <summary>
        /// Creates a new list that is a copy of the original list.
        /// </summary>
        /// <param name="list">The original list to be copied.</param>
        /// <returns>A new list that is a copy of the original list.</returns>
        public static List<T> Clone<T>(this IList<T> list) {
            List<T> newList = new List<T>();
            foreach (T item in list) {
                newList.Add(item);
            }

            return newList;
        }

        /// <summary>
        /// Swaps two elements in the list at the specified indices.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index of the first element.</param>
        /// <param name="indexB">The index of the second element.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }
        
        /// <summary>
        /// Get a random element from <paramref name="list"/>.
        /// </summary>
        public static T GetRandom<T>(this IList<T> list) =>
            list[UnityEngine.Random.Range(0, list.Count)];

        /// <summary>
        /// Based on the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new();

            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];

                do provider.GetBytes(box);
                while (!(box[0] < n * (MaxValue / n)));

                int k = (box[0] % n);
                n--;
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        /// <summary>
        /// Removes a random item from the list, returning that item.
        /// Sampling without replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T RemoveRandom<T>(this IList<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);

            return item;
        }
        
        /// <summary>
        /// Perform and action in each element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of elemnts in the sequence.</typeparam>
        /// <param name="sequence">The sequence to iterate over.</param>
        /// <param name="action">The action to perform over each element.</param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var item in sequence)
                action(item);
        }
    }
}