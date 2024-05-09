using System;
using UnityEngine;

namespace DreamBuilders
{
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