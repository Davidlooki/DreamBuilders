using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinValueAttribute : ValidatorAttribute
    {
        public float MinValue { get; private set; }

        public MinValueAttribute(float minValue) => MinValue = minValue;

        public MinValueAttribute(int minValue) => MinValue = minValue;
    }
}