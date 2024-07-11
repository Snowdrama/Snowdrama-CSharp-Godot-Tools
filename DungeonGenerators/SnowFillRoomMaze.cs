using Godot;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

[Tool]
public partial class SnowFillRoomMaze : Node2D
{
    [Export] int debugScale = 100;
    [Export] float debugLineWidth = 3.0f;
    [Export] bool useArrows = false;
    SnowFillCell[,] debugMaze = new SnowFillCell[16, 16];
    [Export] Vector2I debugMapSize = new Vector2I(16, 16);
    [Export] Vector2I debugRoomSizeMin = new Vector2I(2, 2);
    [Export] Vector2I debugRoomSizeMax = new Vector2I(5, 5);
    [Export] Vector2I debugRoomGapSizeMin = new Vector2I(1, 1);
    [Export] Vector2I debugRoomGapSizeMax = new Vector2I(2, 2);
    [Export] Vector2I debugStartPoint = new Vector2I(8, 8);
    [Export] bool generateDebugMaze;
    Stopwatch benchmarkStopwatch = new Stopwatch();


    public override void _Process(double delta)
    {
        if (generateDebugMaze)
        {
            GD.Print("Generazing Maze");
            generateDebugMaze = false;
            benchmarkStopwatch.Reset();
            benchmarkStopwatch.Start();

            debugMaze = new SnowFillCell[debugMapSize.X, debugMapSize.Y];
            SnowFillMaze.GenerateMaze(ref debugMaze, debugMapSize, debugStartPoint);
            GenerateRooms(ref debugMaze, debugRoomSizeMin, debugRoomSizeMax, debugRoomGapSizeMin, debugRoomGapSizeMax);


            for (int i = 0; i < debugMapSize.X * debugMapSize.Y; i++)
            {
                RemoveDeadEndRoutine(ref debugMaze);
            }

            benchmarkStopwatch.Stop();
            GD.Print($"Generating took {benchmarkStopwatch.Elapsed}");

            QueueRedraw();
        }
    }
    Random rand = new Random();
    public void GenerateRooms(ref SnowFillCell[,] map, Vector2I roomSizeSettingMin, Vector2I roomSizeSettingMax, Vector2I roomGapSizeSettingMin, Vector2I roomGapSizeSettingMax)
    {

        int stepCount = 0;
        int roomIndex = 0;
        while (stepCount < 1000)
        {
            Vector2I roomPosition = new Vector2I(
                rand.Next(0, map.GetLength(0)),
                rand.Next(0, map.GetLength(1))
                );
            Vector2I roomSize = new Vector2I(
                rand.Next(roomSizeSettingMin.X, roomSizeSettingMax.X),
                rand.Next(roomSizeSettingMin.Y, roomSizeSettingMax.Y)
                );
            Vector2I roomGap = new Vector2I(
                rand.Next(roomGapSizeSettingMin.X, roomGapSizeSettingMax.X),
                rand.Next(roomGapSizeSettingMin.Y, roomGapSizeSettingMax.Y)
                );
            if (IsRoomInBound(map, roomPosition, roomSize))
            {
                if (!OverlappingRoom(map, roomPosition, roomSize))
                {
                    GD.Print($"Creating room {roomIndex} at: {roomPosition} sized: {roomSize}");
                    SetRoom(ref map, roomPosition, roomSize, roomGap, roomIndex);
                    roomIndex++;
                }
            }
            stepCount++;
        }

    }
    private static void SetRoom(ref SnowFillCell[,] map, Vector2I roomPosition, Vector2I roomSize, Vector2I gapSetting, int roomId)
    {
        for (int y = -gapSetting.Y; y < roomSize.Y + gapSetting.Y; y++)
        {
            for (int x = -gapSetting.X; x < roomSize.X + gapSetting.X; x++)
            {
                var pos = roomPosition + new Vector2I(x, y);
                if (IsPointInBounds(map, pos))
                {
                    map[pos.X, pos.Y].isRoomGap = true;
                }
            }
        }

        for (int y = 0; y < roomSize.Y; y++)
        {
            for (int x = 0; x < roomSize.X; x++)
            {
                var pos = roomPosition + new Vector2I(x, y);
                if (IsPointInBounds(map, pos))
                {
                    map[pos.X, pos.Y].isIsPartOfRoom = true;
                    map[pos.X, pos.Y].roomId = roomId;
                }
            }
        }
    }
    private static bool OverlappingRoom(SnowFillCell[,] map, Vector2I roomPosition, Vector2I roomSize)
    {
        for (int y = 0; y < roomSize.Y; y++)
        {
            for (int x = 0; x < roomSize.X; x++)
            {
                var offset = new Vector2I(x, y);
                var pos = roomPosition + offset;
                if (map[pos.X, pos.Y].isIsPartOfRoom || map[pos.X, pos.Y].isRoomGap)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool IsRoomInBound(SnowFillCell[,] map, Vector2I roomPosition, Vector2I roomSize)
    {
        if (
            roomPosition.X < 0 ||
            roomPosition.Y < 0 ||
            roomPosition.X + roomSize.X > map.GetLength(0) ||
            roomPosition.Y + roomSize.Y > map.GetLength(1)
        )
        {
            return false;
        }
        return true;
    }
    private static bool IsPointInBounds(SnowFillCell[,] map, Vector2I point)
    {
        if (point.X < 0 || point.Y < 0 || point.X >= map.GetLength(0) || point.Y >= map.GetLength(1))
        {
            return false;
        }
        return true;
    }

    private static void RemoveDeadEndRoutine(ref SnowFillCell[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                var currentPosition = new Vector2I(x, y);

                if (!map[currentPosition.X, currentPosition.Y].isIsPartOfRoom)
                {
                    var down = new Vector2I(0, 1);
                    var up = new Vector2I(0, -1);
                    var right = new Vector2I(1, 0);
                    var left = new Vector2I(-1, 0);

                    var downCell = currentPosition + down;
                    var upCell = currentPosition + up;
                    var leftCell = currentPosition + left;
                    var rightCell = currentPosition + right;

                    var upConnects = CheckConnection(map, upCell, down);
                    var downConnects = CheckConnection(map, downCell, up);
                    var leftConnects = CheckConnection(map, leftCell, right);
                    var rightConnects = CheckConnection(map, rightCell, left);
                    if(!upConnects && !downConnects && !leftConnects && !rightConnects)
                    {
                        //nothing points to us!
                        map[currentPosition.X, currentPosition.Y].removeCell = true;
                    }
                }
            }
        }
    }


    private static bool CheckConnection(SnowFillCell[,] map, Vector2I testCell, Vector2I testDirection)
    {
        if (
            //make sure it's in bounds
            IsPointInBounds(map, testCell) &&
            //if the cell isn't removed
            !map[testCell.X, testCell.Y].removeCell && 
            //if it's pointing towards us
            map[testCell.X, testCell.Y].direction == testDirection
        )
        {
            return true;
        }
        return false;
    }


    public override void _Draw()
    {
        //DrawCircle(cursorPosition * debugScale, 10, Colors.Red, true);

        for (int y = 0; y < debugMaze.GetLength(1); y++)
        {
            for (int x = 0; x < debugMaze.GetLength(0); x++)
            {
                var mazeCell = debugMaze[x, y];

                if (mazeCell.isIsPartOfRoom)
                {
                    var cellSize = new Vector2(debugScale, debugScale);
                    var halfSize = cellSize / 2.0f;
                    DrawRect(new Rect2((mazeCell.position * debugScale) - halfSize, cellSize), Colors.DarkGray, true);
                }
            }
        }

        for (int y = 0; y < debugMaze.GetLength(1); y++)
        {
            for (int x = 0; x < debugMaze.GetLength(0); x++)
            {
                var mazeCell = debugMaze[x, y];
                var from = mazeCell.position * debugScale;
                var to = mazeCell.position * debugScale + mazeCell.direction * debugScale;

                if (mazeCell.removeCell)
                {
                    if (useArrows)
                    {
                        DrawArrow(from, to, Colors.Blue, debugLineWidth);
                    }
                    else
                    {
                        DrawLine(from, to, Colors.Blue, debugLineWidth);
                    }
                }
                else
                {
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


//public struct SnowFillRoom
//{
//    public Vector2I position;
//    public Vector2I size;

//    public bool Intersects(SnowFillRoom room)
//    {
//        if(
//            room.position.X <= this.position.X + this.size.X &&
//            room.position.Y <= this.position.Y + this.size.Y &&

//            this.position.X <= room.position.X + room.size.X &&
//            this.position.Y <= room.position.Y + room.size.Y)
//        {
//            return true;
//        }
//        return false;
//    }


//    public (Vector2I, Vector2I) GetEdge(int index)
//    {
//        switch (index)
//        {
//            case 0:
//                return (new Vector2I(this.position.X, this.position.Y), new Vector2I(this.position.X + this.size.X, this.position.Y));
//            case 1:
//                return (new Vector2I(this.position.X + this.size.X, this.position.Y), new Vector2I(this.position.X + this.size.X, this.position.Y + this.size.Y));
//            case 2:
//                return (new Vector2I(this.position.X, this.position.Y), new Vector2I(this.position.X + this.size.X, this.position.Y));
//            case 3:
//                return (new Vector2I(this.position.X, this.position.Y + this.size.Y), new Vector2I(this.position.X + this.size.X, this.position.Y + this.size.Y));
//        }
//        return (new Vector2I(), new Vector2I());
//    }
//}