using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinValueAttribute : ValidatorAttribute
    {
        #region Fields

        public float MinValue { get; private set; }

        #endregion

        #region Constructors

        public MinValueAttribute(float minValue) => MinValue = minValue;

        public MinValueAttribute(int minValue) => MinValue = minValue;

        #endregion

        #region Methods

        #endregion
    }
}