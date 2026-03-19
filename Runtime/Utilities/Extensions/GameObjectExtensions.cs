using System.Linq;
using UnityEngine;

namespace DreamBuilders
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Hide GameObject in Hierarchy view.
        /// </summary>
        public static void HideInHierarchy(this GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        /// <summary>
        /// Gets a component of the given type attached to the GameObject.
        /// If that type of component does not exist, it adds one.
        /// </summary>
        /// <remarks>
        /// Be aware that a GameObject can hold multiple components of the same type.
        /// </remarks>  
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component =>
            !gameObject.TryGetComponent<T>(out var component)
                ? gameObject.AddComponent<T>()
                : component;

        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        public static void DestroyChildren(this GameObject gameObject) =>
            gameObject.transform.DestroyChildren();

        public static void DestroyChildrenImmediate(this GameObject gameObject) =>
            gameObject.transform.DestroyChildrenImmediate();

        public static void SetChildrenActive(this GameObject gameObject, bool value) =>
            gameObject.transform.SetChildrenActive(value);

        public static void ResetTransform(this GameObject gameObject) =>
            gameObject.transform.Reset();

        /// <summary>
        /// Returns the hierarchical path in the Unity scene hierarchy for this GameObject.
        /// </summary>
        /// <returns>A string representing the full hierarchical path of this GameObject in the Unity scene.
        /// This is a '/'-separated string where each part is the name of a parent, starting from the root parent and ending
        /// with the name of the specified GameObjects parent.</returns>
        public static string Path(this GameObject gameObject) =>
            "/" + string.Join("/",
                gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());

        /// <summary>
        /// Returns the full hierarchical path in the Unity scene hierarchy for this GameObject.
        /// </summary>
        /// <returns>A string representing the full hierarchical path of this GameObject in the Unity scene.
        /// This is a '/'-separated string where each part is the name of a parent, starting from the root parent and ending
        /// with the name of the specified GameObject itself.</returns>
        public static string PathFull(this GameObject gameObject) => gameObject.Path() + "/" + gameObject.name;

        /// <summary>
        /// Recursively sets the provided layer for this GameObject and all of its descendants in the Unity scene hierarchy.
        /// </summary>
        public static void SetLayersRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            gameObject.transform.ForEveryChildDo(child => child.gameObject.SetLayersRecursively(layer));
        }
    }
}