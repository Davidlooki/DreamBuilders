using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MaxValueAttribute : ValidatorAttribute
    {
        #region Fields

        public float MaxValue { get; private set; }

        #endregion

        #region Constructors

        public MaxValueAttribute(float maxValue) => MaxValue = maxValue;
        public MaxValueAttribute(int maxValue) => MaxValue = maxValue;

        #endregion

        #region Methods

        #endregion
    }
}