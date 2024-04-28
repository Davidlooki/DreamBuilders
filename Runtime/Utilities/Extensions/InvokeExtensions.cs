using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DreamBuilders
{
    public static class InvokeExtensions
    {
        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds.
        /// </summary>
        public static Coroutine Invoke(this MonoBehaviour mb, UnityAction method, float time) =>
            mb.StartCoroutine(Invoker(method, time));

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds, then repeatedly every <paramref name="repeatRate"/> seconds.
        /// </summary>
        public static Coroutine InvokeRepeating(
            this MonoBehaviour mb,
            UnityAction method,
            float time,
            float repeatRate
        ) =>
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

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds using unscaled time.
        /// </summary>
        public static Coroutine UnscaledInvoke(this MonoBehaviour mb, UnityAction method, float time) =>
            mb.StartCoroutine(UnscaledInvoker(method, time));

        /// <summary>
        /// Invokes the method <paramref name="method"/> in <paramref name="time"/> seconds, then repeatedly every <paramref name="repeatRate"/> seconds using unscaled time.
        /// </summary>
        public static Coroutine UnscaledInvokeRepeating(
            this MonoBehaviour mb,
            UnityAction method,
            float time,
            float repeatRate
        ) =>
            mb.StartCoroutine(UnscaledInvoker(method, time, repeatRate, true));

        private static IEnumerator UnscaledInvoker(
            UnityAction method,
            float time,
            float repeatRate = 0,
            bool repeat = false
        )
        {
            yield return new WaitForSecondsRealtime(time);
            do
            {
                method();

                yield return new WaitForSecondsRealtime(repeatRate);
            } while (repeat);
        }
    }
}