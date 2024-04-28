using UnityEngine;

namespace DreamBuilders
{
    public static class NumbersExtensions
    {
        public static float PercentageOf(this int part, int whole) =>
            whole == 0
                ? 0 // Handling division by zero
                : (float)part / whole;

        public static bool IsOdd(this int i) => i % 2 == 1;
        public static bool IsEven(this int i) => i % 2 == 0;

        public static int AtLeast(this int value, int min) => Mathf.Max(value, min);
        public static int AtMost(this int value, int max) => Mathf.Min(value, max);

        public static float AtLeast(this float value, float min) => Mathf.Max(value, min);
        public static float AtMost(this float value, float max) => Mathf.Min(value, max);
    }
}