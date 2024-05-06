using System;
using System.Collections.Generic;

namespace DreamBuilders
{
    public static class ListExtensions
    {
        /// <summary>
        /// Removes a random item from the list, returning that item.
        /// Sampling without replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="removedElement">The element removed from sequence.</param>
        /// <returns></returns>
        public static IList<T> RemoveRandom<T>(this IList<T> sequence, out T removedElement)
        {
            removedElement = sequence.GetRandom();
            sequence.Remove(removedElement);

            return sequence;
        }

        /// <summary>
        /// Based on the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        public static IList<T> Shuffle<T>(this IList<T> sequence)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new();

            for (int i = sequence.Count; i > 1; i--)
            {
                byte[] box = new byte[1];

                do provider.GetBytes(box);
                while (!(box[0] < i * (byte.MaxValue / i)));

                int k = (box[0] % i);
                i--;

                sequence.Swap(i, k);
            }

            return sequence;
        }

        /// <summary>
        /// Swaps two elements in the sequence.
        /// </summary>
        public static IList<T> Swap<T>(this IList<T> sequence, int indexA, int indexB)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            if (sequence.Count > indexA && sequence.Count > indexB)
                throw new Exception("One of indexes are greater than given sequence.");

            (sequence[indexA], sequence[indexB]) = (sequence[indexB], sequence[indexA]);

            return sequence;
        }
    }
}