using System;
using UnityEngine;

namespace DreamBuilders
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CurveRangeAttribute : AttributeDrawer
    {
        #region Fields

        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public EColor Color { get; private set; }

        #endregion

        #region Constructors

        public CurveRangeAttribute(Vector2 min, Vector2 max, EColor color = EColor.Clear) =>
            (Min, Max, Color) = (min, max, color);

        public CurveRangeAttribute(EColor color)
            : this(Vector2.zero, Vector2.one, color) { }

        public CurveRangeAttribute(float minX, float minY, float maxX, float maxY, EColor color = EColor.Clear)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY), color) { }

        #endregion

        #region Methods

        #endregion
    }
}