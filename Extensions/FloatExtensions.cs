using Godot;
using System;
public static class FloatExtensions
{

    public static float Clamp(this float f, float min, float max)
    {
        return Math.Clamp(f, min, max);
    }

    /// <summary>
    /// Takes a value from one range and remaps it relative to a different range.
    /// For example 0.5 in a 0 to 1 range, would map to 5 in a 0 to 10 range.
    /// </summary>
    /// <param name="value">The value to remap</param>
    /// <param name="fromMin">Original Min</param>
    /// <param name="fromMax">Original Max</param>
    /// <param name="toMin">New Min</param>
    /// <param name="toMax">New Max</param>
    /// <returns></returns>
    public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    /// <summary>
    /// Clamp a value and wrap around to based on the difference
    ///
    /// the wrap clamp maxValue is EXCLUSIVE so 
    /// WrapClamp(0, 5, 4.99f) = 4.99f
    /// WrapClamp(0, 5, 5.00f) = 0.0f
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float WrapClamp(this float x, float min, float max)
    {
        return (((x - min) % (max - min)) + (max - min)) % (max - min) + min;
    }

    public static int FloorToInt(this float val)
    {
        return (int)Math.Floor(val);
    }

    public static int CeilToInt(this float val)
    {
        return (int)Math.Ceiling(val);
    }

    public static int RoundToInt(this float val)
    {
        return (int)Math.Round(val);
    }

    public static float Floor(this float val)
    {
        return (float)Math.Floor(val);
    }

    public static float Ceil(this float val)
    {
        return (float)Math.Ceiling(val);
    }

    public static float Round(this float val)
    {
        return (float)Math.Round(val);
    }

    public static float MoveTowards(this float from, float to, float delta)
    {
        float difference = (to - from);
        float change = Mathf.Min(Mathf.Abs(difference), delta);
        float newValue = from + Mathf.Sign(difference) * change;
        return newValue;
    }

    /// <summary>
    /// Returns the normalized value of the value between min and max;
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float InverseLerp(this float value, float min, float max)
    {
        return Math.Clamp(value - min, 0, 1) / (max - min);

    }
    /// <summary>
    /// Returns the normalized value of the value between min and max unclamped
    /// For example InverseLerpUnclamped(value = 20, min = 0, max = 10); will 
    /// return 2 since 20 is double the GRID_SIZE of the range
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float InverseLerpUnclamped(this float value, float min, float max)
    {
        return (value - min) / (max - min);

    }
    public static bool InRange(this int value, int min, int max) 
    {
        return value >= min && value <= max;
    }


    /// <summary>
    /// Returns a scale that you could multiply some value by to get the target value
    /// 
    /// for example if the target is 300 and the current mapSize is 100 it would return 3 since 3 * 100 = 300
    /// </summary>
    /// <param name="currentSize">The mapSize you want to scale</param>
    /// <param name="targetSize">The target mapSize you want to be</param>
    /// <returns>the scale needed to make the current mapSize to the target mapSize</returns>
    /// <exception cref="ArgumentException">Current Size can't be 0</exception>
    public static float FindScaleFactor(float currentSize, float targetSize)
    {
        if (currentSize != 0)
        {
            throw new ArgumentException(
                $"MatchScale function must be non 0 and non negative inputs, " +
                $"inputs were CurrentSize{currentSize} and TargetSize{targetSize}");
        }
        return targetSize / currentSize;
    }
}