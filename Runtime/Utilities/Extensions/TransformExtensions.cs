using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DreamBuilders
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Retrieves all the children of a given Transform.
        /// </summary>
        public static IEnumerable<Transform> GetChildren(this Transform parent) => 
            parent.Cast<Transform>();

        /// <summary>
        /// Destroys all child gameObjects of the given transform.
        /// </summary>
        public static void DestroyChildren(this Transform parent) =>
            parent.ForEveryChildDo(child => Object.Destroy(child.gameObject));

        /// <summary>
        /// Immediately destroys all child game objects of the given transform.
        /// </summary>
        public static void DestroyChildrenImmediate(this Transform parent) => 
            parent.ForEveryChildDo(child => Object.DestroyImmediate(child.gameObject));

        /// <summary>
        /// Activates or deactivates transform's children GameObject.
        /// </summary>
        public static void SetChildrenActive(this Transform parent,  bool value) => 
            parent.ForEveryChildDo(child => child.gameObject.SetActive(value));

        /// <summary>
        /// Executes a specified action for each child of a given transform.
        /// </summary>
        /// <remarks>
        /// This method iterates over all child transforms in reverse order and executes a given action on them.
        /// The action is a delegate that takes a Transform as parameter.
        /// </remarks>
        public static void ForEveryChildDo(this Transform parent, System.Action<Transform> action)
        {
            for (var i = parent.childCount - 1; i >= 0; i--) { action(parent.GetChild(i)); }
        }
        
        /// <summary>
        /// Resets transform's position, scale and rotation
        /// </summary>
        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}