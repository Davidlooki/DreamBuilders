using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamBuilders
{
    public static class ArraysExtensions
    {
        /// <summary>
        /// Based on the Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        public static T[] Shuffle<T>(this T[] sequence)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new();

            for (int i = sequence.Length; i > 1; i--)
            {
                byte[] box = new byte[1];

                do provider.GetBytes(box);
                while (!(box[0] < i * (byte.MaxValue / i)));

                int k = (box[0] % i);
                i--;

                sequence.Swap(i, k);
            }

            return sequence.ToArray();
        }

        /// <summary>
        /// Swaps two elements in the sequence.
        /// </summary>
        public static T[] Swap<T>(this T[] sequence, int indexA, int indexB)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            if (sequence.Length > indexA && sequence.Length > indexB)
                throw new Exception("One of indexes are greater than given sequence.");

            (sequence[indexA], sequence[indexB]) = (sequence[indexB], sequence[indexA]);

            return sequence;
        }
    }
}