using System;
using UnityEngine;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AnimatorParamAttribute : AttributeDrawer
    {
        #region Fields

        public string AnimatorName { get; private set; }
        public AnimatorControllerParameterType? AnimatorParamType { get; private set; }

        #endregion

        #region Constructors

        public AnimatorParamAttribute(string animatorName) => AnimatorName = animatorName;

        public AnimatorParamAttribute(string animatorName, AnimatorControllerParameterType animatorParamType) =>
            (AnimatorName, AnimatorParamType) = (animatorName, animatorParamType);

        #endregion

        #region Methods

        #endregion
    }
}