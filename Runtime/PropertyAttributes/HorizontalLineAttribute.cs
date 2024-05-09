using System;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class HorizontalLineAttribute : AttributeDrawer
    {
        public const float DefaultHeight = 2.0f;
        public const EColor DefaultColor = EColor.Gray;

        public float Height { get; private set; }
        public EColor Color { get; private set; }

        public HorizontalLineAttribute(float height = DefaultHeight, EColor color = DefaultColor) =>
            (Height, Color) = (height, color);
    }
}