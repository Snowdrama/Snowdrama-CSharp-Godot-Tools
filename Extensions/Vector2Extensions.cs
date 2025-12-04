using Godot;
using Snowdrama.Core;
using System;
public static class Vector2Extensions
{
    private static Random rand = new Random();
    private static float PI = 3.14159265358979f;
    private static float Deg2Rad = PI / 180;
    private static float Rad2Deg = 180 / PI;
    public static Vector2 VectorFromAngleRads(float angle)
    {
        Vector2 V = new Vector2();
        V.X = Mathf.Cos(angle);
        V.Y = Mathf.Sin(angle);
        return V.Normalized();
    }
    public static Vector2 VectorFromAngleDegrees(float angle)
    {
        return VectorFromAngleRads(Deg2Rad * angle);
    }

    public static Vector2I FloorToInt(this Vector2 dir)
    {
        return new Vector2I(Mathf.FloorToInt(dir.X), Mathf.FloorToInt(dir.Y));
    }

    public static Vector2I CeilToInt(this Vector2 dir)
    {
        return new Vector2I(Mathf.CeilToInt(dir.X), Mathf.CeilToInt(dir.Y));
    }

    public static Vector2I RoundToInt(this Vector2 dir)
    {
        return new Vector2I(Mathf.RoundToInt(dir.X), Mathf.RoundToInt(dir.Y));
    }

    public static Vector2 Floor(this Vector2 dir)
    {
        return new Vector2(Mathf.Floor(dir.X), Mathf.Floor(dir.Y));
    }

    public static Vector2 Ceil(this Vector2 dir)
    {
        return new Vector2(Mathf.Ceil(dir.X), Mathf.Ceil(dir.Y));
    }

    public static Vector2 Round(this Vector2 dir)
    {
        return new Vector2(Mathf.Round(dir.X), Mathf.Round(dir.Y));
    }

    public static Vector2I Clamp(this Vector2I val, Vector2I min, Vector2I max)
    {
        return new Vector2I(Mathf.Clamp(val.X, min.X, max.X), Mathf.Clamp(val.Y, min.Y, max.Y));
    }
    public static Vector2 Clamp(this Vector2 val, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(val.X, min.X, max.X), Mathf.Clamp(val.Y, min.Y, max.Y));
    }

    public static float RandomBetweenXY(this Vector2 val)
    {
        return RandomAndNoise.RandomRange(val.X, val.Y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val">The current position</param>
    /// <param name="magnitude">the distance from the original position that you want the point</param>
    /// <returns>A new position some distance away from the original position</returns>
    public static Vector2 RandomDirectionOffset(this Vector2 val, float magnitude = 1.0f)
    {
        return val + (RandomDirection() * magnitude);
    }

    public static Vector2 RandomDirection()
    {
        return Vector2.Right.Rotated(RandomAndNoise.RandomAngle()).Normalized();
    }

    public static Vector2 Random(float minX, float maxX, float minY, float maxY, bool normalized = true)
    {
        if (normalized)
        {
            return new Vector2((float)Mathf.Lerp(minX, maxX, rand.NextDouble()), (float)Mathf.Lerp(minY, maxY, rand.NextDouble())).Normalized();
        }
        return new Vector2((float)Mathf.Lerp(minX, maxX, rand.NextDouble()), (float)Mathf.Lerp(minY, maxY, rand.NextDouble()));
    }

    /// <summary>
    /// Gets a point that is the combination of the smallest value of both pointArr 
    /// 
    /// For example if you have the pointArr (3,5) and (2,9) the result would be (2, 5)
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns>a new vector that has the smallest value of both pointArr</returns>
    public static Vector2 Min(Vector2 A, Vector2 B)
    {
        return new Vector2(Mathf.Min(A.X, B.X), Mathf.Min(A.Y, B.Y));
    }

    /// <summary>
    /// Gets a point that is the combination of the largest value of both pointArr 
    /// 
    /// For example if you have the pointArr (3,5) and (2,9) the result would be (3, 9)
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns>a new vector that has the largest value of both pointArr</returns>
    public static Vector2 Max(Vector2 A, Vector2 B)
    {
        return new Vector2(Mathf.Max(A.X, B.X), Mathf.Max(A.Y, B.Y));
    }


    public static bool InBounds(this Vector2 pos, Vector2 TopLeftPos, Vector2 BottomRightPos)
    {
        if (pos.X >= TopLeftPos.X &&
            pos.X < BottomRightPos.X &&
            pos.Y >= TopLeftPos.Y &&
            pos.Y < BottomRightPos.Y)
        {
            return true;
        }
        return false;
    }
    public static bool InBounds(this Vector2I pos, Vector2I TopLeftPos, Vector2I BottomRightPos)
    {
        if (pos.X >= TopLeftPos.X &&
            pos.X < BottomRightPos.X &&
            pos.Y >= TopLeftPos.Y &&
            pos.Y < BottomRightPos.Y)
        {
            return true;
        }
        return false;
    }
    public static bool InBounds(this Vector2I pos, Vector2 TopLeftPos, Vector2 BottomRightPos)
    {
        return InBounds(pos, TopLeftPos.RoundToInt(), BottomRightPos.RoundToInt());
    }

    public static float AngleFromVectorDegrees(this Vector2 dir)
    {
        return Rad2Deg * dir.AngleFromVectorRads();
    }

    public static float AngleFromVectorRads(this Vector2 dir)
    {
        var angle = Mathf.Atan2(dir.Y, dir.X);
        if (angle < 0)
        {
            angle += 2 * PI;
        }
        return angle;
    }

    public static Vector2 PerpendicularClockwise(this Vector2 vec)
    {
        return new Vector2(vec.Y, -vec.X);
    }
    public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
    {
        return new Vector2(Mathf.Lerp(start.X, end.X, t), Mathf.Lerp(start.Y, end.Y, t));
    }

    public static Vector2 PerpendicularCounterClockwise(this Vector2 vec)
    {
        return new Vector2(-vec.Y, vec.X);
    }
    public static float AngleTo(this Vector2 self, Vector2 to)
    {
        Vector2 direction = to - self;
        float angle = Mathf.Atan2(direction.Y, direction.X) * Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }


    public static Vector2 MoveTowards(this Vector2 from, Vector2 to, float delta)
    {
        Vector2 translation = (to - from);

        float scale = Mathf.Min(translation.Length(), delta);

        Vector2 newVector = from + (translation.Normalized() * scale);

        return newVector;
    }
    public static Vector2 FindScaleFactor(this Vector2 size, Vector2 targetSize, bool stretchToFit = false)
    {
        if (!stretchToFit)
        {
            var fullScale = targetSize / size;
            var minimumNeededToFit = Mathf.Min(fullScale.X, fullScale.Y);
            return new Vector2(minimumNeededToFit, minimumNeededToFit);
        }
        return targetSize / size;
    }
}