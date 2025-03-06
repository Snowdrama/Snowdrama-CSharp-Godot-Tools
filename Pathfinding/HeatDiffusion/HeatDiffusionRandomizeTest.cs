using Godot;
using Snowdrama.Core;
using System;

public partial class HeatDiffusionRandomizeTest : Node2D
{
    [Export] Vector2 min = new Vector2(0, 0);
    [Export] Vector2 max = new Vector2(5000, 5000);
    public override void _Ready()
    {
        base._Ready();
        this.GlobalPosition = RandomAndNoise.RandomPosition(min, max);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_select"))
        {
            this.GlobalPosition = RandomAndNoise.RandomPosition(min, max);
        }
    }
}
