using System.Collections;
using System.Collections.Generic;
using Godot;

public static class Vector3Extensions
{
    public static Vector3I FloorToInt(this Vector3 dir)
    {
        return new Vector3I(Mathf.FloorToInt(dir.X), Mathf.FloorToInt(dir.Y), Mathf.FloorToInt(dir.Z));
    }

    public static Vector3I CeilToInt(this Vector3 dir)
    {
        return new Vector3I(Mathf.CeilToInt(dir.X), Mathf.CeilToInt(dir.Y), Mathf.CeilToInt(dir.Z));
    }

    public static Vector3I RoundToInt(this Vector3 dir)
    {
        return new Vector3I(Mathf.RoundToInt(dir.X), Mathf.RoundToInt(dir.Y), Mathf.RoundToInt(dir.Z));
    }

    public static Vector3 Floor(this Vector3 dir)
    {
        return new Vector3(Mathf.Floor(dir.X), Mathf.Floor(dir.Y), Mathf.Floor(dir.Z));
    }

    public static Vector3 Ceil(this Vector3 dir)
    {
        return new Vector3(Mathf.Ceil(dir.X), Mathf.Ceil(dir.Y), Mathf.Ceil(dir.Z));
    }

    public static Vector3 Round(this Vector3 dir)
    {
        return new Vector3(Mathf.Round(dir.X), Mathf.Round(dir.Y), Mathf.Round(dir.Z));
    }

    public static Vector3 Lerp(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(Mathf.Lerp(start.X, end.X, t), Mathf.Lerp(start.Y, end.Y, t), Mathf.Lerp(start.Z, end.Z, t));
    }
    public static Vector3I Clamp(this Vector3I val, Vector3I min, Vector3I max)
    {
        return new Vector3I(Mathf.Clamp(val.X, min.X, max.X), Mathf.Clamp(val.Y, min.Y, max.Y), Mathf.Clamp(val.Z, min.Z, max.Z));
    }
    public static Vector3 Clamp(this Vector3 val, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(val.X, min.X, max.X), Mathf.Clamp(val.Y, min.Y, max.Y), Mathf.Clamp(val.Z, min.Z, max.Z));
    }


    //
    public static Vector3 MoveTowards(this Vector3 from, Vector3 to, float delta)
    {
        Vector3 translation = (to - from) ;

        float scale = Mathf.Min(translation.Length(), delta);

        Vector3 newVector = from + (translation.Normalized() * scale);

        return newVector;
    }
}
