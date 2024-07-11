using Godot;
using System;

[Tool]
public partial class MazeGenerator_MazeRewriter : Node2D
{
    [Export] Vector2I mapSize = new Vector2I(16, 16);
    [Export] Vector2I minMapSize = new Vector2I(16, 16);
    [Export] Vector2I maxMapSize = new Vector2I(64, 64);

    Vector2I[,] mazeDirections = new Vector2I[16, 16];

    Vector2I cursorPosition = new Vector2I(0,0);

    [Export] bool create;
    int stepCount = 0;
    int targetStepCount = 2560;

    Vector2I[] directions = new Vector2I[]
    {
        new Vector2I(1, 0),
        new Vector2I(0, 1),
        new Vector2I(-1, 0),
        new Vector2I(0, -1),
    };
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (create)
        {
            create = false;
            GenerazeMaze();
            QueueRedraw();
        }
    }

    public void GenerazeMaze()
    {
        CreatePerfectMaze(new Vector2I(mapSize.X, mapSize.Y));

        stepCount = 0;
        cursorPosition = new Vector2I();
        targetStepCount = mazeDirections.GetLength(0) * mazeDirections.GetLength(1) * 10;

        while (stepCount < targetStepCount)
        {
            //point the origin to a random neighbor
            var nextDirection = directions.GetRandom();

            //check if it's in bounds
            while (!IsInBounds(mazeDirections, cursorPosition + nextDirection))
            {
                nextDirection = directions.GetRandom();
            }

            //set the target from this space to the adjacent space
            mazeDirections[cursorPosition.X, cursorPosition.Y] = cursorPosition + nextDirection;
            
            //move the cursor
            cursorPosition = cursorPosition + nextDirection;

            //set the tile to point to itself since it's the cursor position
            mazeDirections[cursorPosition.X, cursorPosition.Y] = cursorPosition;

            stepCount++;
        }
    }
    public void CreatePerfectMaze(Vector2I size)
    {
        mazeDirections = new Vector2I[size.X, size.Y];
        for (int y = 0; y < size.Y; y++)
        {
            for (int x = 0; x < size.X; x++)
            {
                if (x == cursorPosition.X && y == cursorPosition.Y)
                {
                    mazeDirections[x, y] = new Vector2I(0, 0);
                }
                if (x == 0)
                {
                    mazeDirections[x, y] = new Vector2I(x, y + -1);
                }
                else
                {
                    mazeDirections[x, y] = new Vector2I(x - 1, y);
                }
            }
        }
    }

    private bool IsInBounds(Vector2I[,] map, Vector2I point)
    {
        if(point.X < 0 || point.Y < 0 || point.X >= map.GetLength(0) || point.Y >= map.GetLength(1))
        {
            return false;
        }
        return true;
    }

    [ExportGroup("Debug")]
    [Export] int debugScale = 100;
    [Export] float debugLineWidth = 3.0f;
    [Export] bool useArrows = false;
    public override void _Draw()
    {
        DrawCircle(cursorPosition * debugScale, 10, Colors.Red, true);

        for (int y = 0; y < mazeDirections.GetLength(1); y++)
        {
            for (int x = 0; x < mazeDirections.GetLength(0); x++)
            {
                var from = new Vector2I(x, y) * debugScale;
                var to = mazeDirections[x, y] * debugScale;
                if (useArrows)
                {
                    DrawArrow(from, to, Colors.Blue, debugLineWidth);
                }
                else
                {
                    DrawLine(from, to, Colors.Blue, debugLineWidth);
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
