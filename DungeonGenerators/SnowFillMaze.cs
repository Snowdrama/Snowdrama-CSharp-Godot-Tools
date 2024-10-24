using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;
using System.Transactions;

public class Directions2D
{
    private static Vector2I[] directions = new Vector2I[]
    {
        Vector2I.Left,
        Vector2I.Right,
        Vector2I.Up,
        Vector2I.Down,
    };
    public static Vector2I GetRandomDirection()
    {
        return directions.GetRandom();
    }
}

public class SnowFillMaze
{
    public static readonly Vector2I DOWN = new Vector2I(0, 1);
    public static readonly Vector2I UP = new Vector2I(0, -1);
    public static readonly Vector2I RIGHT = new Vector2I(1, 0);
    public static readonly Vector2I LEFT = new Vector2I(-1, 0);

    public static void GenerateMaze(ref SnowFillCell2D[,] maze, Vector2I size, Vector2I startPoint)
    {
        maze = new SnowFillCell2D[size.X, size.Y];

        var filledCount = 0;
        var cellCount = size.X * size.Y;

        CreateMaze(ref maze, size, startPoint);
        FloodFillRoutine(ref maze);

        var stepCount = 0;
        var targetStepCount = 1000;

        while (stepCount < targetStepCount && filledCount < cellCount)
        {
            RandomizeDisconnectedCells(ref maze);
            FloodFillRoutine(ref maze);
            filledCount = CountFilled(maze);
            if (filledCount >= cellCount)
            {
                break;
            }
            stepCount++;
        }

        //finally hook up all directions
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            for (int x = 0; x < maze.GetLength(0); x++)
            {
                Vector2I currentPosition = new Vector2I(x, y);
                bool connecedUp = CheckValidPathInDirection(maze, currentPosition, UP);
                bool connecedDown = CheckValidPathInDirection(maze, currentPosition, DOWN);
                bool connecedLeft = CheckValidPathInDirection(maze, currentPosition, LEFT);
                bool connecedRight = CheckValidPathInDirection(maze, currentPosition, RIGHT);

                if (connecedUp)
                {
                    maze[currentPosition.X, currentPosition.Y].connectedDirections.Add(UP);
                }
                if (connecedDown)
                {
                    maze[currentPosition.X, currentPosition.Y].connectedDirections.Add(DOWN);
                }
                if (connecedLeft)
                {
                    maze[currentPosition.X, currentPosition.Y].connectedDirections.Add(LEFT);
                }
                if (connecedRight)
                {
                    maze[currentPosition.X, currentPosition.Y].connectedDirections.Add(RIGHT);
                }
            }
        }


        GD.Print($"Finshed in {stepCount} steps");
    }

    private static void CreateMaze(ref SnowFillCell2D[,] map, Vector2I size, Vector2I start)
    {
        map = new SnowFillCell2D[size.X, size.Y];
        GD.Print($"Starting Maze of mapSize {new Vector2(map.GetLength(0), map.GetLength(1))} ");
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                map[x, y].connectedDirections = new Array<Vector2I>();
                map[x, y].extraProperties_int = new Dictionary<string, int>();
                map[x, y].extraProperties_float = new Dictionary<string, float>();
                map[x, y].extraProperties_bool = new Dictionary<string, bool>();
                map[x, y].extraProperties_string = new Dictionary<string, string>();

                map[x, y].position = new Vector2I(x, y);
                map[x, y].direction = Directions2D.GetRandomDirection();
                if (map[x, y].position == start)
                {
                    GD.Print($"Starting Cell is {start}");
                    map[x, y].isConnected = true;
                }
                else
                {
                    map[x, y].isConnected = false;
                }
            }
        }
    }

    private static void RandomizeDisconnectedCells(ref SnowFillCell2D[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y].isConnected == false)
                {
                    map[x, y].direction = Directions2D.GetRandomDirection();
                }
            }
        }
    }

    private static void FloodFillRoutine(ref SnowFillCell2D[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                //we flood fill from each cell, if the cell isn't connected
                //we escape early anyway
                FloodFill(map, new Vector2I(x, y), 0);
            }
        }
    }


    private static void FloodFill(SnowFillCell2D[,] map, Vector2I currentPosition, int depth)
    {
        //prevent going super deep
        //TODO: This really should be like mapSize.X * mapSize.Y?
        if (depth > 100)
        {
            return;
        }

        //don't check neighbors if we're not connected, we'll check when we get to a connected node
        if (!map[currentPosition.X, currentPosition.Y].isConnected)
        {
            return;
        }

        //check each direction and if it points to me, then we should mark as connected
        var down = new Vector2I(0, 1);
        var up = new Vector2I(0, -1);
        var right = new Vector2I(1, 0);
        var left = new Vector2I(-1, 0);

        var downCell = currentPosition + down;
        var upCell = currentPosition + up;
        var leftCell = currentPosition + left;
        var rightCell = currentPosition + right;

        //check if the nodes near me are pointing to me
        CheckConnection(map, upCell, down, depth);
        CheckConnection(map, downCell, up, depth);
        CheckConnection(map, leftCell, right, depth);
        CheckConnection(map, rightCell, left, depth);
    }

    private static void CheckConnection(SnowFillCell2D[,] map, Vector2I testCell, Vector2I testDirection, int depth)
    {
        if (IsInBounds(map, testCell) && !map[testCell.X, testCell.Y].isConnected && map[testCell.X, testCell.Y].direction == testDirection)
        {
            map[testCell.X, testCell.Y].isConnected = true;
            FloodFill(map, testCell, depth++);
        }
    }

    private static bool CheckValidPathInDirection(SnowFillCell2D[,] map, Vector2I testCell, Vector2I testDirection)
    {
        //are we both in bounds?
        if (IsInBounds(map, testCell) && IsInBounds(map, testCell+testDirection))
        {
            //do we point to them?
            if(map[testCell.X, testCell.Y].direction == testDirection)
            {
                return true;
            }
            //or do they point to us
            if (map[testCell.X + testDirection.X, testCell.Y + testDirection.Y].direction == -testDirection)
            {
                return true;
            }
        }
        return false;
    }

    private static int CountFilled(SnowFillCell2D[,] map)
    {
        int count = 0;
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y].isConnected)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool IsInBounds(SnowFillCell2D[,] map, Vector2I point)
    {
        if (point.X < 0 || point.Y < 0 || point.X >= map.GetLength(0) || point.Y >= map.GetLength(1))
        {
            return false;
        }
        return true;
    }


}
public struct SnowFillCell2D
{
    public bool removeCell;

    public Vector2I position;
    public bool isConnected;
    public Vector2I direction;


    public bool isIsPartOfRoom;
    public bool isRoomGap;
    public int roomId;

    public Array<Vector2I> connectedDirections;

    public Dictionary<string, int> extraProperties_int;
    public Dictionary<string, float> extraProperties_float;
    public Dictionary<string, bool> extraProperties_bool;
    public Dictionary<string, string> extraProperties_string;
}