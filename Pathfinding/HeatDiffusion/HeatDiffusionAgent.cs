using Godot;
using System;
using static Godot.TextServer;

[GlobalClass]
public partial class HeatDiffusionAgent : Node2D
{
    private Font _defaultFont = ThemeDB.FallbackFont;
    [Export] HeatDiffusionMap heatMap;
    [Export] float speed = 2;
    [Export(PropertyHint.Range, "0.01, 0.5")] float chaseThreshold = 0.25f;
    CharacterBody2D body;
    public override void _Ready()
    {
        base._Ready();
        body = this.GetParent<CharacterBody2D>();
        if (body == null)
        {
            GD.PrintErr($"HeatDiffusionAgent {this.GetParent().Name} is not a CharacterBody2D");
        }
    }

    double waitTime = 0.0f;
    [Export] double waitTimeMax = 0.25f;
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (heatMap == null)
        {
            return;
        }


        var targetCell = heatMap.GetHeatGradientTarget(body.GlobalPosition, chaseThreshold) + (heatMap.CellSize * 0.5f);
        var direction = this.GlobalPosition.DirectionTo(targetCell);
        var dist = this.GlobalPosition.DistanceSquaredTo(targetCell);
        if (dist < 50.0f)
        {
            body.Velocity = direction.Normalized() * speed * 0.25f;
        }
        else
        {
            body.Velocity = direction.Normalized() * speed;
        }
        body.MoveAndSlide();
        QueueRedraw();
    }

    public override void _Draw()
    {
        //DrawLine(Vector2.Zero, body.Velocity, Colors.Red);
        //var directions = heatMap.GetHeatGradientDirectionList(body.GlobalPosition);
        //foreach (var direction in directions)
        //{
        //    DrawString(_defaultFont, (direction.Item3 * 50), $"[{direction.Item2}]");
        //    DrawString(_defaultFont, (direction.Item3 * 50) + new Vector2(0, 15), $"{direction.Item1:F2}");
        //}
        //DrawString(_defaultFont, new Vector2(0, 100), $"[{heatMap.GetCell(this.GlobalPosition)}]");
    }

    public void SetMap(HeatDiffusionMap map)
    {
        heatMap = map;
    }
}
