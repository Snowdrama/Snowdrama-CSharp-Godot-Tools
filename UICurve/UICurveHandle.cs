using Godot;
using System;

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
            parentCurve.UpdateCurve();
            oldPos = this.Position;
        }
    }
}
