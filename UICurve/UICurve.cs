using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

[Tool, GlobalClass]
public partial class UICurve : Resource
{
    //[Tooltip("Control pointArr in screen percentage (0 to 1)")]
    [Export] public Array<Vector2> controlPoints = new Array<Vector2>();

    /// <summary>
    /// Evaluate the curve at normalized time t [0, 1]
    /// </summary>
    public Vector2 Evaluate(float t)
    {
        if (controlPoints == null || controlPoints.Count < 2)
        {
            Debug.LogWarning("UICurve requires at least 2 control pointArr.");
            controlPoints.Add(new Vector2(0, 0));
            controlPoints.Add(new Vector2(1, 1));
            return Vector2.Zero;
        }

        int segmentCount = controlPoints.Count - 1;
        float scaledT = t * segmentCount;
        int i = Mathf.FloorToInt(scaledT);
        i = Mathf.Clamp(i, 0, segmentCount - 1);

        float localT = scaledT - i;

        Vector2 p0 = GetPoint(i - 1);
        Vector2 p1 = GetPoint(i);
        Vector2 p2 = GetPoint(i + 1);
        Vector2 p3 = GetPoint(i + 2);

        Vector2 interpolated = CatmullRom(p0, p1, p2, p3, localT);
        return interpolated;
    }

    public Vector2 EvaluateScreen(float t, Control containerUIRect)
    {
        var screenPos = Evaluate(t);
        Vector2 anchoredPos;

        //convert from screen position to world position in px

        var rect = containerUIRect.GetRect();
        var rectSize = rect.Size;
        var rectPos = rect.Position;

        //size = 100, 100
        //screnPos = 0.5, 0.5
        var offset = new Vector2(screenPos.X * rectSize.X, screenPos.Y * rectSize.Y);
        anchoredPos = rectPos + offset;

        return anchoredPos;
    }

    /// <summary>
    /// Clamp index and get control point
    /// </summary>
    private Vector2 GetPoint(int index)
    {
        index = Mathf.Clamp(index, 0, controlPoints.Count - 1);
        return controlPoints[index];
    }

    /// <summary>
    /// Convert (0-1) percentage to screen space position
    /// </summary>
    private Vector2 PercentageToScreenPosition(Vector2 percent)
    {
        return new Vector2(percent.X * DisplayServer.WindowGetSize().X, percent.Y * DisplayServer.WindowGetSize().Y);
    }

    /// <summary>
    /// Catmull-Rom spline interpolation
    /// </summary>
    private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }
}
