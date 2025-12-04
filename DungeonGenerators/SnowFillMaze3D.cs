using Godot;

public partial class SnowFillMaze3D : Node
{

    private static Vector3I[] directions = new Vector3I[]
    {
        Vector3I.Right,
        Vector3I.Left,
        Vector3I.Up,
        Vector3I.Down,
        Vector3I.Back,
        Vector3I.Forward
    };

    public static void GenerateMaze(ref SnowFillCell3D[,,] maze, Vector3I size, Vector3I startPoint)
    {
        maze = new SnowFillCell3D[size.X, size.Y, size.Z];

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
        Debug.Log($"Finshed in {stepCount} steps");
    }

    private static void CreateMaze(ref SnowFillCell3D[,,] map, Vector3I size, Vector3I start)
    {
        map = new SnowFillCell3D[size.X, size.Y, size.Z];
        Debug.Log($"Starting Maze of mapSize {new Vector2(map.GetLength(0), map.GetLength(1))} ");
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int z = 0; z < map.GetLength(2); z++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y, z].position = new Vector3I(x, y, z);
                    map[x, y, z].direction = directions.GetRandom();
                    if (map[x, y, z].position == start)
                    {
                        Debug.Log($"Starting Cell is {start}");
                        map[x, y, z].isConnected = true;
                    }
                    else
                    {
                        map[x, y, z].isConnected = false;
                    }
                }
            }
        }
    }

    private static void RandomizeDisconnectedCells(ref SnowFillCell3D[,,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int z = 0; z < map.GetLength(2); z++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y, z].isConnected == false)
                    {
                        map[x, y, z].direction = directions.GetRandom();
                    }
                }
            }
        }
    }

    private static void FloodFillRoutine(ref SnowFillCell3D[,,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int z = 0; z < map.GetLength(2); z++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    //we flood fill from each cell, if the cell isn't connected
                    //we escape early anyway
                    FloodFill(map, new Vector3I(x, y, z), 0);
                }
            }
        }
    }


    private static void FloodFill(SnowFillCell3D[,,] map, Vector3I currentPosition, int depth)
    {
        //prevent going super deep
        //TODO: This really should be like mapSize.X * mapSize.Y?
        if (depth > 100)
        {
            return;
        }

        //don't check neighbors if we're not connected, we'll check when we get to a connected node
        if (!map[currentPosition.X, currentPosition.Y, currentPosition.Z].isConnected)
        {
            return;
        }

        //check each direction and if it pointArr to me, then we should mark as connected
        var down = Vector3I.Down;
        var up = Vector3I.Up;
        var right = Vector3I.Right;
        var left = Vector3I.Left;

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

    private static void CheckConnection(SnowFillCell3D[,,] map, Vector3I testCell, Vector3I testDirection, int depth)
    {
        if (IsInBounds(map, testCell) && !map[testCell.X, testCell.Y, testCell.Z].isConnected && map[testCell.X, testCell.Y, testCell.Z].direction == testDirection)
        {
            map[testCell.X, testCell.Y, testCell.Z].isConnected = true;
            FloodFill(map, testCell, depth++);
        }
    }

    private static int CountFilled(SnowFillCell3D[,,] map)
    {
        int count = 0;
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int z = 0; z < map.GetLength(2); z++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    if (map[x, y, z].isConnected)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private static bool IsInBounds(SnowFillCell3D[,,] map, Vector3I point)
    {
        if (point.X < 0 || point.Y < 0 || point.X >= map.GetLength(0) || point.Y >= map.GetLength(1))
        {
            return false;
        }
        return true;
    }
}

public struct SnowFillCell3D
{
    public bool removeCell;

    public Vector3I position;
    public bool isConnected;
    public Vector3I direction;


    public bool isIsPartOfRoom;
    public bool isRoomGap;
    public int roomId;
}