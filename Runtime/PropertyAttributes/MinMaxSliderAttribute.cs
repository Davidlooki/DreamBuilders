using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxSliderAttribute : AttributeDrawer
    {
        #region Fields

        public float MinimumValue { get; private set; }
        public float MaximumValue { get; private set; }

        #endregion

        #region Constructors

        public MinMaxSliderAttribute(float minimumValue, float maximumValue) =>
            (MinimumValue, MaximumValue) = (minimumValue, maximumValue);

        #endregion

        #region Methods

        #endregion
    }
}