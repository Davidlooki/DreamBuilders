using UnityEngine;

public class MathX
{
    #region Methods

    #region Hermite

    /// <summary>
    ///  Interpolate while easing in and out at the limits.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float Hermite(float start, float end, float value) =>
        Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));

    /// <summary>
    /// Interpolate while easing in and out at the limits.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 Hermite(Vector2 start, Vector2 end, float value) =>
        new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));

    /// <summary>
    /// Interpolate while easing in and out at the limits.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector3 Hermite(Vector3 start, Vector3 end, float value) =>
        new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));

    #endregion

    #region Sinerp

    /// <summary>
    /// Sinusoidal interpolation while easing around the end, when value is near one.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float Sinerp(float start, float end, float value) =>
        Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));

    /// <summary>
    /// Sinusoidal interpolation while easing around the end, when value is near one.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 Sinerp(Vector2 start, Vector2 end, float value) =>
        new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
            Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)));

    /// <summary>
    /// Sinusoidal interpolation easing out when value is near one.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector3 Sinerp(Vector3 start, Vector3 end, float value) =>
        new(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
            Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)),
            Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f)));

    #endregion

    #region Coserp

    //Ease in
    /// <summary>
    /// Cosenoidal interpolation easing in when value is near zero.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float Coserp(float start, float end, float value) =>
        Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));

    /// <summary>
    /// Cosenoidal interpolation easing in when value is near zero.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 Coserp(Vector2 start, Vector2 end, float value) =>
        new(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));

    /// <summary>
    /// Cosenoidal interpolation easing in when value is near zero.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector3 Coserp(Vector3 start, Vector3 end, float value) =>
        new(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));

    #endregion

    #region Berp

    /// <summary>
    /// Boing-like interpolation overshoot, then waver back and forth around the end value before coming to a rest.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
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
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector2 Berp(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
    }

    /// <summary>
    /// Boing-like interpolation overshoot, then waver back and forth around the end value before coming to a rest.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector3 Berp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
    }

    #endregion

    #region SmoothStep

    /// <summary>
    /// Like Lerp, but has ease-in and ease-out of the values.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
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
    /// <param name="vec"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector2 SmoothStep(Vector2 vec, float min, float max)
    {
        return new Vector2(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max));
    }

    /// <summary>
    /// Like Lerp, but has ease-in and ease-out of the values.
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector3 SmoothStep(Vector3 vec, float min, float max)
    {
        return new Vector3(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max));
    }

    #endregion

    #region NearestPoint

    /// <summary>
    /// Returns the nearest point on a line to a point.
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="point"></param>
    /// <example>Useful for making an object follow a track</example>
    /// <returns></returns>
    public static Vector2 NearestPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    {
        Vector2 lineDirection = (lineEnd - lineStart).normalized;
        float closestPoint = Vector2.Dot((point - lineStart), lineDirection);

        return lineStart + (closestPoint * lineDirection);
    }

    /// <summary>
    /// Returns the nearest point on a line to a point.
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="point"></param>
    /// <example>Useful for making an object follow a track</example>
    /// <returns></returns>
    public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
        float closestPoint = Vector3.Dot((point - lineStart), lineDirection);

        return lineStart + (closestPoint * lineDirection);
    }

    #endregion

    #region NearestPointStrict

    /// <summary>
    /// Works like NearestPoint except the end of the line is clamped.
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="point"></param>
    /// <returns></returns>
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
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    /// <param name="point"></param>
    /// <returns></returns>
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
    /// <param name="x"></param>
    /// <returns></returns>
    public static float Bounce(float x)
    {
        return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
    }

    /// <summary>
    /// Returns a value between 0 and 1 that can be used to easily make bouncing.
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector2 Bounce(Vector2 vec)
    {
        return new Vector2(Bounce(vec.x), Bounce(vec.y));
    }

    /// <summary>
    /// Returns a value between 0 and 1 that can be used to easily make bouncing.
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3 Bounce(Vector3 vec)
    {
        return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
    }

    #endregion

    #region Approx

    /// <summary>
    /// Test for value that is near specified float (due to floating point inprecision).
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static bool Approx(float point1, float point2) => Mathf.Approximately(point1, point2);

    /// <summary>
    /// Test if a Vector3 is close to another Vector3 (due to floating point inprecision).
    /// Compares the square of the distance to the square of the range as this avoids calculating a square root which is
    /// much slower than squaring the range.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="about"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static bool Approx(Vector3 val, Vector3 about, float range) =>
        (val - about).sqrMagnitude < range * range;

    #endregion

    #region Clerp

    /// <summary>
    /// Circular Lerp is Like lerp but handles the wraparound from 0 to 360.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="value"></param>
    /// <example>Useful when interpolating eulerAngles and the object crosses the 0/360 boundary.</example>
    /// <returns></returns>
    public static float Clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min) / 2.0f); //half the distance between min and max
        float retval = 0.0f;
        float diff = 0.0f;

        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else
            retval = start + (end - start) * value;

        return retval;
    }

    #endregion

    #region Etc

    public static double Sigmoid(double input, double coeficient = 1) =>
        (1 / (1 + System.Math.Exp(-input * coeficient)));

    #endregion

    #endregion
}