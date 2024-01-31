using Godot;
using System;

[Tool, GlobalClass]
public partial class PathDrawer2D : Path2D
{
    [Export] int splineLength = 100;
    [Export] bool _smooth = true;
    [Export] bool _straighten = false;
    public void Straighten()
    {
        for (int i = 0; i < Curve.PointCount; i++)
        {
            Curve.SetPointIn(i, new Vector2());
            Curve.SetPointIn(i, new Vector2());
        }
    }
    public void Smooth()
    {
        for (int i = 0; i < Curve.PointCount; i++)
        {
            var spline = GetSpline(i);
            Curve.SetPointIn(i, -spline);
            Curve.SetPointIn(i, spline);
        }
    }
    public Vector2 GetSpline(int index)
    {
        var lastPoint = GetPoint(index - 1);
        var nextPoint = GetPoint(index + 1);
        var spline = lastPoint.DirectionTo(nextPoint) * splineLength;
        return spline;
    }
    public Vector2 GetPoint(int index)
    {
        var pointCount = Curve.PointCount;
        index = Mathf.Wrap(index, 0, pointCount - 1);
        return Curve.GetPointPosition(index);
    }

    public override void _Draw()
    {
        base._Draw();
        var points = Curve.GetBakedPoints();
        if(points != null)
        {
            DrawPolyline(points, Color.FromHtml("#0080FF"), 8, true);
        }
    }
}