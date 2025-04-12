using Godot;

namespace Snowdrama.Core;

[Tool]
public partial class UICurveHandle : Node2D
{
    Vector2 oldPos;
    [Export] public UICurveNode parentCurve;

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (parentCurve == null) { return; }
        if (this.Position != oldPos)
        {
            this.Position = this.Position.Clamp(Vector2.Zero, parentCurve.Size);

            parentCurve.UpdateCurve();
            oldPos = this.Position;
        }
        QueueRedraw();
    }
    public override void _Draw()
    {
        base._Draw();

        if (GodotTools.IsNodeSelectedEditor(this))
        {
            var drawOffset = parentCurve.GlobalPosition - this.GlobalPosition;
            DrawRect(new Rect2()
            {
                Position = drawOffset,
                Size = parentCurve.GetRect().Size,
            }, Colors.OrangeRed, false, 2.0f, true);


            if (parentCurve.Curve == null) { return; }
            for (int i = 0; i < parentCurve.Curve.controlPoints.Count; i++)
            {
                var controlPoint = parentCurve.Curve.controlPoints[i];
                var worldPoint = parentCurve.Curve.controlPoints[i] * parentCurve.Size;
                DrawCircle(worldPoint + drawOffset, 10.0f, Colors.Cyan, false, 5.0f, true);
            }

            int rez = parentCurve.Curve.controlPoints.Count * 10;
            for (float c = 0; c < rez; c++)
            {
                float t1 = c / rez;
                float t2 = (c + 1) / rez;
                var worldPoint1 = parentCurve.Curve.Evaluate(t1) * parentCurve.Size;
                var worldPoint2 = parentCurve.Curve.Evaluate(t2) * parentCurve.Size;
                DrawLine(worldPoint1 + drawOffset, worldPoint2 + drawOffset, Colors.CornflowerBlue, 5.0f, true);
            }
        }
    }
}
