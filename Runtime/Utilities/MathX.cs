using UnityEngine;

namespace DreamBuilders
{
    public static class MathX
    {
        #region Hermite

        /// <summary>
        ///  Interpolate while easing in and out at the limits.
        /// </summary>
        public static float Hermite(float start, float end, float value) =>
            Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));

        /// <summary>
        /// Interpolate while easing in and out at the limits.
        /// </summary>
        public static Vector2 Hermite(Vector2 start, Vector2 end, float value) =>
            new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));

        /// <summary>
        /// Interpolate while easing in and out at the limits.
        /// </summary>
        public static Vector3 Hermite(Vector3 start, Vector3 end, float value) =>
            new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));

        #endregion

        #region Sinerp

        /// <summary>
        /// Sinusoidal interpolation while easing around the end, when value is near one.
        /// </summary>
        public static float Sinerp(float start, float end, float value) =>
            Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));

        /// <summary>
        /// Sinusoidal interpolation while easing around the end, when value is near one.
        /// </summary>
        public static Vector2 Sinerp(Vector2 start, Vector2 end, float value) =>
            new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
                Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)));

        /// <summary>
        /// Sinusoidal interpolation easing out when value is near one.
        /// </summary>
        public static Vector3 Sinerp(Vector3 start, Vector3 end, float value) =>
            new(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
                Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)),
                Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f)));

        #endregion

        #region Coserp

        /// <summary>
        /// Cosenoidal interpolation easing in when value is near zero.
        /// </summary>
        public static float Coserp(float start, float end, float value) =>
            Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));

        /// <summary>
        /// Cosenoidal interpolation easing in when value is near zero.
        /// </summary>
        public static Vector2 Coserp(Vector2 start, Vector2 end, float value) =>
            new(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));

        /// <summary>
        /// Cosenoidal interpolation easing in when value is near zero.
        /// </summary>
        public static Vector3 Coserp(Vector3 start, Vector3 end, float value) =>
            new(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));

        #endregion

        #region Berp

        /// <summary>
        /// Boing-like interpolation overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        public static float Berp(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) +
                     value) * (1f + (1.2f * (1f - value)));

            return start + (end - start) * value;
        }

        /// <summary>
        /// Boing-like interpolation overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        public static Vector2 Berp(Vector2 start, Vector2 end, float value) =>
            new(Berp(start.x, end.x, value), Berp(start.y, end.y, value));

        /// <summary>
        /// Boing-like interpolation overshoot, then waver back and forth around the end value before coming to a rest.
        /// </summary>
        public static Vector3 Berp(Vector3 start, Vector3 end, float value) =>
            new(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));

        #endregion

        #region SmoothStep

        /// <summary>
        /// Like Lerp, but has ease-in and ease-out of the values.
        /// </summary>
        public static float SmoothStep(float x, float min, float max)
        {
            x = Mathf.Clamp(x, min, max);
            float v1 = (x - min) / (max - min);
            float v2 = (x - min) / (max - min);

            return -2 * v1 * v1 * v1 + 3 * v2 * v2;
        }

        /// <summary>
        /// Like Lerp, but has ease-in and ease-out of the values.
        /// </summary>
        public static Vector2 SmoothStep(Vector2 vector, float min, float max)
        {
            return new Vector2(SmoothStep(vector.x, min, max), SmoothStep(vector.y, min, max));
        }

        /// <summary>
        /// Like Lerp, but has ease-in and ease-out of the values.
        /// </summary>
        public static Vector3 SmoothStep(Vector3 vector, float min, float max) =>
            new(SmoothStep(vector.x, min, max), SmoothStep(vector.y, min, max), SmoothStep(vector.z, min, max));

        #endregion

        #region NearestPoint

        /// <summary>
        /// Returns the nearest point on a line to a point.
        /// </summary>
        /// <example>Useful for making an object follow a track.</example>
        public static Vector2 NearestPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 lineDirection = (lineEnd - lineStart).normalized;
            float closestPoint = Vector2.Dot((point - lineStart), lineDirection);

            return lineStart + (closestPoint * lineDirection);
        }

        /// <summary>
        /// Returns the nearest point on a line to a point.
        /// </summary>
        /// <example>Useful for making an object follow a track</example>
        public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection);

            return lineStart + (closestPoint * lineDirection);
        }

        /// <summary>
        /// Works like NearestPoint except the end of the line is clamped.
        /// </summary>
        public static Vector2 NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 fullDirection = lineEnd - lineStart;
            Vector2 lineDirection = fullDirection.normalized;
            float closestPoint = Vector2.Dot((point - lineStart), lineDirection);

            return lineStart + (Mathf.Clamp(closestPoint, 0.0f, fullDirection.magnitude) * lineDirection);
        }

        /// <summary>
        /// Works like NearestPoint except the end of the line is clamped.
        /// </summary>
        public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 fullDirection = lineEnd - lineStart;
            Vector3 lineDirection = Vector3.Normalize(fullDirection);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection);

            return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Returns a value between 0 and 1 that can be used to easily make bouncing.
        /// </summary>
        public static float Bounce(float x) =>
            Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));

        /// <summary>
        /// Returns a value between 0 and 1 that can be used to easily make bouncing.
        /// </summary>
        public static Vector2 Bounce(Vector2 vector) =>
            new(Bounce(vector.x), Bounce(vector.y));

        /// <summary>
        /// Returns a value between 0 and 1 that can be used to easily make bouncing.
        /// </summary>
        public static Vector3 Bounce(Vector3 vector) =>
            new(Bounce(vector.x), Bounce(vector.y), Bounce(vector.z));

        #endregion

        /// <summary>
        /// Test for value that is near specified float (due to floating point imprecision).
        /// </summary>
        public static bool Approx(float x1, float x2) =>
            Mathf.Approximately(x1, x2);

        /// <summary>
        /// Circular Lerp is Like lerp but handles the wraparound from 0 to 360.
        /// </summary>
        /// <example>Useful when interpolating eulerAngles and the object crosses the 0/360 boundary.</example>
        public static float CLerp(float start, float end, float value)
        {
            const float MIN = 0.0f;
            const float MAX = 360.0f;
            float half = Mathf.Abs((MAX - MIN) / 2.0f); //half the distance between min and max
            float retVal = 0.0f;
            float diff = 0.0f;

            if ((end - start) < -half)
            {
                diff = ((MAX - start) + end) * value;
                retVal = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((MAX - end) + start) * value;
                retVal = start + diff;
            }
            else
                retVal = start + (end - start) * value;

            return retVal;
        }

        public static double Sigmoid(double input, double coefficient = 1) =>
            1 / (1 + System.Math.Exp(-input * coefficient));
    }
}