using System;
using UnityEngine;

namespace DreamBuilders
{
    /// <summary>
    /// Allow the selection of an <see cref="Animator"/>'s parameter on Inspector through a dropdown list.
    /// </summary>
    /// <remarks>
    /// Works on int and string fields.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class AnimatorParamExample : MonoBehaviour
    /// {
    ///     private Animator _animator;
    ///     
    ///     [AnimatorParam(nameof(_animator))]
    ///     public int ParamHash;
    ///     
    ///     [AnimatorParam(nameof(_animator))]
    ///     public string ParamName;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    public class AnimatorParamAttribute : AttributeDrawer
    {
        public string AnimatorName { get; private set; }
        public AnimatorControllerParameterType? AnimatorParamType { get; private set; }

        public AnimatorParamAttribute(string animatorName) => AnimatorName = animatorName;

        public AnimatorParamAttribute(string animatorName, AnimatorControllerParameterType animatorParamType) =>
            (AnimatorName, AnimatorParamType) = (animatorName, animatorParamType);
    }
}