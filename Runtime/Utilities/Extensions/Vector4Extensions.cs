using UnityEngine;

namespace DreamBuilders
{
    public static class Vector4Extensions
    {
        public static Vector4 Random(this Vector4 vector, float maxRange, float minRange) =>
            new(UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange));
    }
}