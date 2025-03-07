using Godot;
using System;
using static Godot.TextServer;

[GlobalClass]
public partial class HeatDiffusionAgent : Node2D
{
    private Font _defaultFont = ThemeDB.FallbackFont;
    [Export] HeatDiffusionMap heatMap;
    [Export] float speed = 2;
    [Export] float stoppingDistance = 2.0f;
    [Export(PropertyHint.Range, "0.0, 0.5")] float chaseThreshold = 0.25f;
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


        //var targetCell = heatMap.GetHeatGradientTarget(body.GlobalPosition, chaseThreshold) + (heatMap.CellSize * 0.5f);
        //var direction = this.GlobalPosition.DirectionTo(targetCell);
        //var dist = this.GlobalPosition.DistanceSquaredTo(targetCell);
        (currentPathPositions, currentPathCells) = heatMap.GetHeatGradintPath(body.GlobalPosition);

        direction = heatMap.GetHeatGradientDirection(body.GlobalPosition);

        if (currentPathCells.Length > 0)
        {
            targetCell = currentPathCells[0];
        }
        if(currentPathPositions.Length > 0)
        {
            targetPosition = currentPathPositions[0];
            direction = this.GlobalPosition.DirectionTo(targetPosition);
            distance = this.GlobalPosition.DistanceTo(targetPosition);
        }

        if (distance > stoppingDistance)
        {
            body.Velocity = direction * speed;
        }
        else
        {
            body.Velocity = Vector2.Zero;
        }
        

        body.MoveAndSlide();
        QueueRedraw();
    }

    Vector2I[] currentPathCells;
    Vector2[] currentPathPositions;
    Vector2I targetCell;
    Vector2 targetPosition;
    Vector2 direction;
    float distance;

    [Export] bool debug = false;
    public override void _Draw()
    {
        if (debug)
        {
            var drawDirPos = Vector2.Zero;
            if (currentPathCells == null || currentPathCells.Length == 0)
            {
                return;
            }
            //for (int i = 0; i < currentPathCells.Length; i++)
            //{
            //    DrawLine(drawDirPos, currentPathCells[i] * 50, Colors.White);
            //    drawDirPos += currentPathCells[i] * 50;
            //}
            float offset = 0;
            for (int i = 0; i < currentPathCells.Length; i++)
            {
                offset += 15;
                DrawString(_defaultFont, new Vector2(0, offset), $"[{currentPathCells[i]:F0}]");
            }

            offset += 15;
            DrawString(_defaultFont, new Vector2(0, offset), $"[Distance]: {distance}");
            //for (int i = 0; i < currentPathCells.Length; i++)
            //{
            //    offset += 15;
            //    DrawString(_defaultFont, new Vector2(10, offset), $"[{currentPathPositions[i].FloorToInt():F0}]");
            //}
            DrawLine(Vector2.Zero, direction * 50, Colors.White);
            //DrawCircle(ToLocal(targetCell), 10.0f, Colors.White, false);
            //DrawLine(Vector2.Zero, body.Velocity, Colors.Red);
            //var directions = heatMap.GetHeatGradientDirectionList(body.GlobalPosition);
            //foreach (var direction in directions)
            //{
            //    DrawString(_defaultFont, (direction.Item3 * 50), $"[{direction.Item2}]");
            //    DrawString(_defaultFont, (direction.Item3 * 50) + new Vector2(0, 15), $"{direction.Item1:F2}");
            //}
            //DrawString(_defaultFont, new Vector2(0, 100), $"[{heatMap.GetCellFromPosition(this.GlobalPosition)}]");
        }
    }

    public void SetMap(HeatDiffusionMap map)
    {
        heatMap = map;
    }
}
