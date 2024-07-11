using Godot;
using System;
using System.Diagnostics;


[Tool]
public partial class SnowFillMazeDebug : Node2D
{
    [Export] int debugScale = 100;
    [Export] float debugLineWidth = 3.0f;
    [Export] bool useArrows = false;
    SnowFillCell[,] debugMaze = new SnowFillCell[16, 16];
    [Export] Vector2I debugMapSize = new Vector2I(16, 16);
    [Export] Vector2I debugStartPoint = new Vector2I(8, 8);
    [Export] bool generateDebugMaze;
    Stopwatch benchmarkStopwatch = new Stopwatch();

	public override void _Process(double delta)
    {
        if (generateDebugMaze)
        {
            benchmarkStopwatch.Reset();
            benchmarkStopwatch.Start();
            generateDebugMaze = false;

            debugMaze = new SnowFillCell[16, 16];
            SnowFillMaze.GenerateMaze(ref debugMaze, debugMapSize, debugStartPoint);
            benchmarkStopwatch.Stop();
            GD.Print($"Generating took {benchmarkStopwatch.Elapsed}");
        }
    }


    public override void _Draw()
    {
        //DrawCircle(cursorPosition * debugScale, 10, Colors.Red, true);

        for (int y = 0; y < debugMaze.GetLength(1); y++)
        {
            for (int x = 0; x < debugMaze.GetLength(0); x++)
            {
                var mazeCell = debugMaze[x, y];
                var from = mazeCell.position * debugScale;
                var to = mazeCell.position * debugScale + mazeCell.direction * debugScale;

                if (useArrows)
                {
                    if (mazeCell.isConnected)
                    {
                        DrawArrow(from, to, Colors.Green, debugLineWidth);
                    }
                    else
                    {
                        DrawArrow(from, to, Colors.Blue, debugLineWidth);
                    }
                }
                else
                {
                    if (mazeCell.isConnected)
                    {
                        DrawLine(from, to, Colors.Green, debugLineWidth);
                    }
                    else
                    {
                        DrawLine(from, to, Colors.Blue, debugLineWidth);
                    }
                }
            }
        }
    }
    private void DrawArrow(Vector2 from, Vector2 to, Color color, float width = 1.0f)
    {
        var direction = from.DirectionTo(to) * 100;
        var arrowLeft = to + (direction.Normalized() * 10).Rotated(Mathf.DegToRad(-160));
        var arrowRight = to + (direction.Normalized() * 10).Rotated(Mathf.DegToRad(160));


        DrawLine(from, to, color, width);
        DrawLine(to, arrowLeft, color, width);
        DrawLine(to, arrowRight, color, width);
    }
}
