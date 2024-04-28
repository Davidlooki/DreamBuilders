namespace DreamBuilders
{
    public static class ArraysExtensions
    {
        /// <summary>
        /// Get a random element from <paramref name="array"/>.
        /// </summary>
        public static T GetRandom<T>(this T[] array) =>
            array[UnityEngine.Random.Range(0, array.Length)];
    }
}