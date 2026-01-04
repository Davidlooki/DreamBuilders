using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxSliderAttribute : AttributeDrawer
    {
        public float MinimumValue { get; private set; }
        public float MaximumValue { get; private set; }

        public MinMaxSliderAttribute(float minimumValue, float maximumValue) =>
            (MinimumValue, MaximumValue) = (minimumValue, maximumValue);
    }
}