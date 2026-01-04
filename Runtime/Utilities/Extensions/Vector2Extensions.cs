using UnityEngine;

namespace DreamBuilders
{
    public static class Vector2Extensions
    {
        public static Vector2 Random(this Vector2 vector, float maxRange, float minRange) =>
            new(UnityEngine.Random.Range(minRange, maxRange),
                UnityEngine.Random.Range(minRange, maxRange));
        
        /// <summary>
        /// Adds to any x y values of a Vector2
        /// </summary>
        public static Vector2 Add(this Vector2 vector, float x = 0, float y = 0) => 
            new(vector.x + x, vector.y + y);

        /// <summary>
        /// Sets any x y values of a Vector2
        /// </summary>
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null) => 
            new(x ?? vector.x, y ?? vector.y);

        /// <summary>
        /// Returns a Boolean indicating whether the current Vector2 is in a given range from another Vector2
        /// </summary>
        /// <param name="current">The current Vector2 position</param>
        /// <param name="target">The Vector2 position to compare against</param>
        /// <param name="range">The range value to compare against</param>
        /// <returns>True if the current Vector2 is in the given range from the target Vector2, false otherwise</returns>
        public static bool InRangeOf(this Vector2 current, Vector2 target, float range) => 
            (current - target).sqrMagnitude <= range * range;
    }
}