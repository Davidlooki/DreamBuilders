using System;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;
using UnityAction = UnityEngine.Events.UnityAction;
using UnityEngine;
using static System.Byte;

namespace DreamBuilders
{
    public static class Extensions
    {
        #region Invoke

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds.
        /// </summary>
        public static Coroutine Invoke(this MonoBehaviour mb, UnityAction method, float time) =>
            mb.StartCoroutine(Invoker(method, time));

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds, then repeatedly every <paramref name="repeatRate"/> seconds.
        /// </summary>
        public static Coroutine
            InvokeRepeating(this MonoBehaviour mb, UnityAction method, float time, float repeatRate) =>
            mb.StartCoroutine(Invoker(method, time, repeatRate, true));

        private static IEnumerator Invoker(UnityAction method, float time, float repeatRate = 0, bool repeat = false)
        {
            yield return new WaitForSeconds(time);
            do
            {
                method();
                yield return new WaitForSeconds(repeatRate);
            } while (repeat);
        }

        #endregion

        #region Unscaled Invoke

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds using unscaled time.
        /// </summary>
        public static Coroutine UnscaledInvoke(this MonoBehaviour mb, UnityAction method, float time) =>
            mb.StartCoroutine(UnscaledInvoker(method, time));

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds, then repeatedly every <paramref name="repeatRate"/> seconds using unscaled time.
        /// </summary>
        public static Coroutine UnscaledInvokeRepeating(this MonoBehaviour mb, UnityAction method, float time,
                                                        float repeatRate) =>
            mb.StartCoroutine(UnscaledInvoker(method, time, repeatRate, true));

        private static IEnumerator UnscaledInvoker(UnityAction method, float time, float repeatRate = 0,
                                                   bool repeat = false)
        {
            yield return new WaitForSecondsRealtime(time);
            do
            {
                method();
                yield return new WaitForSecondsRealtime(repeatRate);
            } while (repeat);
        }

        #endregion

        #region Numbers

        public static double Sigmoid(this double input, double coeficient = 1) =>
            (1 / (1 + System.Math.Exp(-input * coeficient)));

        #endregion

        #region Strings

        /// <summary>
        /// Reverse a String
        /// </summary>
        /// <param name="str">The string to Reverse</param>
        /// <returns>The reversed String</returns>
        public static string Reverse(this string str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        public static int WordCount(this string str) =>
            str.Split(new char[] {' ', '.', '?'}, StringSplitOptions.RemoveEmptyEntries).Length;

        public static bool IsValidString(this string str, char[] prohibitedChars, short maxChar = 0) =>
            (maxChar <= 0 || str.Length <= maxChar) && !string.IsNullOrEmpty(str) &&
            str.All(inputChar => prohibitedChars.All(prohibitedChar => inputChar != prohibitedChar));

        public static int ToInt(this string str)
        {
            if (!int.TryParse(str, out int result))
                result = 0;

            return result;
        }

        public static float ToFloat(this string str)
        {
            if (!float.TryParse(str, out float result))
                result = 0;

            return result;
        }

        public static T ToEnum<T>(this string str, bool ignorecase = true)
        {
            if (str == null)
                throw new ArgumentNullException("value");

            str = str.Trim();

            if (str.Length == 0)
                throw new ArgumentException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);

            return !t.IsEnum
                ? throw new ArgumentException("Type provided must be an Enum.", "T")
                : (T) Enum.Parse(t, str, ignorecase);
        }

        #endregion

        #region Enums

        #endregion

        #region Bools

        public static bool IsOneOf<T>(this T item, params T[] options) =>
            Array.Exists(options, x => x.Equals(item));

        #endregion

        #region Arrays and Lists

        /// <summary>
        /// Get a random element from <paramref name="list"/>.
        /// </summary>
        public static T GetRandom<T>(this IList<T> list) =>
            list[UnityEngine.Random.Range(0, list.Count)];

        /// <summary>
        /// Get a random element from <paramref name="array"/>.
        /// </summary>
        public static T GetRandom<T>(this T[] array) =>
            array[UnityEngine.Random.Range(0, array.Length)];

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

        #endregion

        #region Vectors

        public static Vector2 Random(this Vector2 vector, float maxRange, float minRange) =>
            new(UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange));

        public static Vector3 Random(this Vector3 vector, float maxRange, float minRange) =>
            new(UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange));

        public static Vector4 Random(this Vector4 vector, float maxRange, float minRange) =>
            new(UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange));

        #endregion

        #region AI

        public static bool HasArrived(this UnityEngine.AI.NavMeshAgent navMeshAgent, float tolerance = 0.01f) =>
            !navMeshAgent.pathPending && (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                                          (!navMeshAgent.hasPath ||
                                           navMeshAgent.velocity.sqrMagnitude < tolerance));

        #endregion
    }
}