using Godot;
using Godot.Collections;
using Snowdrama.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

/// <summary>
/// Based slightly on a video I cannot find anymore about using graphs to handle all kinds of things like 
/// faction influence, weather, and pathfinding
/// 
/// 
/// This is essentially a layered gradient descent algorithm, or something like an implementation of flowfields
/// Creating a graph of weights that moves towards some points.
/// 
/// This implementation I'm calling Heat Diffusion as you can define some constant of flow
/// and the heat transfer will take some percentage of the adjacent tiles and average them together
/// with that constant. 
/// 
/// My initial use case was for pathfinding, where I wanted to be able to do pathinding for swarms without needing to do
/// A* for every entity, and considered hierarchical flow fields for this.
/// 
/// However in my case I didn't inherently want multiple flow fields, my entities were dumb and wanted to just move towards
/// things they want to destroy players/objects/walls etc. whatever was destructible. So I considered what if I just 
/// every frame did very small fast calculations for all the tiles.
/// 
/// TODO: This really is suited for compute shaders so I should move this there at some point.
/// 

[GlobalClass]
public partial class HeatDiffusionMap : Node2D
{
    static readonly Vector2I N = new Vector2I(0, -1);
    static readonly Vector2I S = new Vector2I(0, 1);
    static readonly Vector2I E = new Vector2I(-1, 0);
    static readonly Vector2I W = new Vector2I(1, 0);
    static readonly Vector2I NE = new Vector2I(1, -1);
    static readonly Vector2I NW = new Vector2I(-1, -1);
    static readonly Vector2I SE = new Vector2I(1, 1);
    static readonly Vector2I SW = new Vector2I(-1, 1);

    [Export(PropertyHint.Range, "0.5, 2.0, 0.00001")] float constantFalloff = 0.75f;
    [Export] Array<HeatDiffusionObject> heatGeneratorObjects = new Array<HeatDiffusionObject>();
    [Export] Array<HeatDiffusionBlocker> heatBlockerObjects = new Array<HeatDiffusionBlocker>();
    //float[,] heatGenerators = new float[100, 100];
    float[,] heatMap = new float[100, 100];
    bool[,] ignoreMap = new bool[100, 100];
    bool[,] generatorMap = new bool[100, 100];
    [Export] Vector2I size = new Vector2I(100, 100);

    [Export] public Vector2 CellSize = new Vector2I(50, 50);
    [ExportGroup("Debug")]
    [Export] float colorBoost = 1.0f;
    public void AddHeatGeneratorObject(HeatDiffusionObject node)
    {
        heatGeneratorObjects.Add(node);
    }
    public void SetHeatGenerator(Vector2I position, float amount)
    {
        //heatGenerators[position.X, position.Y] = amount;
        ignoreMap[position.X, position.Y] = true;
    }
    public void SetIgnore(Vector2I position, bool ignore)
    {
        ignoreMap[position.X, position.Y] = ignore;
    }

    public override void _Ready()
    {
        base._Ready();
        //heatGenerators = new float[size.X, size.Y];
        heatMap = new float[size.X, size.Y];
        ignoreMap = new bool[size.X, size.Y];
        generatorMap = new bool[size.X, size.Y];

        //for (int i = 0; i < 4; i++)
        //{
        //    var randomX = RandomAndNoise.RandomRange(0, heatMap.GetLength(0));
        //    var randomY = RandomAndNoise.RandomRange(0, heatMap.GetLength(1));
        //    SetHeatGenerator(new Vector2I(randomX, randomY), 1.0f);
        //}

        //for (int y = 0; y < heatMap.GetLength(1); y++)
        //{
        //    var line = "";
        //    for (int x = 0; x < heatMap.GetLength(0); x++)
        //    {
        //        line += $"{heatGenerators[x, y]}";
        //    }
        //    GD.Print(line);
        //}
    }

    Stopwatch sw = new Stopwatch();
    public override void _Process(double delta)
    {
        //if (Input.IsActionPressed("ui_select"))
        //{
        //}

        //sw.Restart();
        UpdateTick();
        //sw.Stop();

        //Debug.Log($"Stopwatch: {sw.ElapsedMilliseconds}");

        QueueRedraw();
    }

    public void UpdateTick()
    {

        //loop over all ignore maps setting them to false
        for (int y = 0; y < generatorMap.GetLength(1); y++)
        {
            for (int x = 0; x < generatorMap.GetLength(0); x++)
            {
                generatorMap[x, y] = false;
            }
        }
        //set every heat generator to it's heat value
        foreach (var item in heatGeneratorObjects)
        {
            Vector2I cellPosition = (item.GlobalPosition / CellSize).FloorToInt();

            if(heatMap.IsIndexInBounds(cellPosition))
            {
                heatMap[cellPosition.X, cellPosition.Y] = item.generatedHeat;
                generatorMap[cellPosition.X, cellPosition.Y] = true;
            }
        }

        //loop over all ignore maps setting them to false
        for (int y = 0; y < ignoreMap.GetLength(1); y++)
        {
            for (int x = 0; x < ignoreMap.GetLength(0); x++)
            {
                ignoreMap[x, y] = false;
            }
        }

        //then set them with the blockers
        foreach (var item in heatBlockerObjects)
        {
            foreach(var blockOffset in item.blockerOffsets)
            {
                Vector2I cellPosition = ((item.GlobalPosition + blockOffset) / CellSize).FloorToInt();

                if (heatMap.IsIndexInBounds(cellPosition))
                {
                    ignoreMap[cellPosition.X, cellPosition.Y] = true;
                    heatMap[cellPosition.X, cellPosition.Y] = 0.0f;
                }
            }
        }


        //average the states of the tiles. 
        for (int y = 0; y < heatMap.GetLength(1); y++)
        {
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                heatMap = AverageCells(new Vector2I(x, y), heatMap, ignoreMap, constantFalloff);
            }
        }

        //for (int y = 0; y < heatMap.GetLength(1); y++)
        //{
        //    for (int x = 0; x < heatMap.GetLength(0); x++)
        //    {
        //        if (heatGenerators[x, y] > 0.1f)
        //        {
        //            heatMap[x, y] = heatGenerators[x, y];
        //        }
        //    }
        //}
    }

    //[Export] float CellSize.X = 25;
    [Export] float cellOffset = 30;

    [Export] float minThing = 0.2f;
    [Export] float maxThing = 1.2f;

    [Export] float minThing1 = 0.2f;
    [Export] float maxThing1 = 1.2f;
    [Export] Color landColor = Colors.DarkGreen;
    [Export] Color visionColor = Colors.CornflowerBlue;
    public override void _Draw()
    {
        base._Draw();

        for (int y = 0; y < heatMap.GetLength(1); y++)
        {
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                var val = Mathf.Lerp(minThing1, maxThing1, Mathf.InverseLerp(minThing, maxThing, heatMap[x, y])).Clamp(0, 1.0f);
                var colorLerp = landColor.Lerp(visionColor, val);
                DrawRect(
                    new Rect2(
                        x * CellSize.X,
                        y * CellSize.Y,
                        CellSize.X,
                        CellSize.Y
                    ),
                    colorLerp
                );
                if (drawText)
                {
                    DrawString(
                        _defaultFont,
                        new Vector2(
                            x * CellSize.X + 5,
                            (y * CellSize.Y) + 15
                        ),
                        $"{heatMap[x, y]:F2}"
                    );
                }
                if (drawCell)
                {
                    DrawString(
                        _defaultFont,
                        new Vector2(
                            x * CellSize.X + 5,
                            (y * CellSize.Y) + 15
                        ),
                        $"\n[{x}, {y}]"
                    );

                }
            }
        }

        foreach (var item in heatGeneratorObjects)
        {
            Vector2I cellPosition = (item.GlobalPosition / CellSize.Y).FloorToInt();

            DrawString(
                _defaultFont,
                new Vector2(
                    cellPosition.X * CellSize.X + 5,
                    (cellPosition.Y * CellSize.Y) + 45
                ),
                $"[{cellPosition.X}, {cellPosition.Y}]"
            );
            if (heatMap.IsIndexInBounds(cellPosition))
            {
                heatMap[cellPosition.X, cellPosition.Y] = item.generatedHeat;
            }
        }
    }
    [Export] bool drawText = false;
    [Export] bool drawCell = true;
    private Font _defaultFont = ThemeDB.FallbackFont;
    [Export] bool eightWayAverage = true;
    [Export] bool eightWayMovement = true;
    private float[,] AverageCells(Vector2I cellPos, float[,] cells, bool[,] ignore, float constant = 1.0f)
    {
        var totalValue = cells[cellPos.X, cellPos.Y];
        var cellValue = cells[cellPos.X, cellPos.Y];
        float cellCount = 1;

        if (!generatorMap[cellPos.X, cellPos.Y] && !ignoreMap[cellPos.X, cellPos.Y])
        {
            if (ignoreMap.IsIndexInBounds(cellPos + N) && !ignore[cellPos.X + N.X, cellPos.Y + N.Y] && cells.TryGetValue(cellPos + N, out cellValue)) { totalValue += cellValue; cellCount++; }
            if (ignoreMap.IsIndexInBounds(cellPos + S) && !ignore[cellPos.X + S.X, cellPos.Y + S.Y] && cells.TryGetValue(cellPos + S, out cellValue)) { totalValue += cellValue; cellCount++; }
            if (ignoreMap.IsIndexInBounds(cellPos + E) && !ignore[cellPos.X + E.X, cellPos.Y + E.Y] && cells.TryGetValue(cellPos + E, out cellValue)) { totalValue += cellValue; cellCount++; }
            if (ignoreMap.IsIndexInBounds(cellPos + W) && !ignore[cellPos.X + W.X, cellPos.Y + W.Y] && cells.TryGetValue(cellPos + W, out cellValue)) { totalValue += cellValue; cellCount++; }

            if (eightWayAverage)
            {
                if (ignoreMap.IsIndexInBounds(cellPos + NE) && !ignore[cellPos.X + NE.X, cellPos.Y + NE.Y] && cells.TryGetValue(cellPos + NE, out cellValue)) { totalValue += cellValue; cellCount++; }
                if (ignoreMap.IsIndexInBounds(cellPos + NW) && !ignore[cellPos.X + NW.X, cellPos.Y + NW.Y] && cells.TryGetValue(cellPos + NW, out cellValue)) { totalValue += cellValue; cellCount++; }
                if (ignoreMap.IsIndexInBounds(cellPos + SE) && !ignore[cellPos.X + SE.X, cellPos.Y + SE.Y] && cells.TryGetValue(cellPos + SE, out cellValue)) { totalValue += cellValue; cellCount++; }
                if (ignoreMap.IsIndexInBounds(cellPos + SW) && !ignore[cellPos.X + SW.X, cellPos.Y + SW.Y] && cells.TryGetValue(cellPos + SW, out cellValue)) { totalValue += cellValue; cellCount++; }
            }
            var test = (totalValue * constant) / cellCount;
            cells[cellPos.X, cellPos.Y] = test;
        }
        return cells;
    }

    //public Vector2 GetHeatGradientDirection(Vector2 pos)
    //{
    //    Vector2I cellPos = (pos / CellSize).FloorToInt();

    //    if (!heatMap.IsIndexInBounds(cellPos))
    //    {
    //        return Vector2.Zero;
    //    }

    //    var averageDirectionTotal = Vector2.Zero;
    //    float currentCellValue = heatMap[cellPos.X, cellPos.Y];
    //    float cellValue = 0;
    //    float cellCount = 0;
    //    float highestValue = heatMap[cellPos.X, cellPos.Y];
    //    Vector2 highestCell = cellPos;
    //    Vector2 highestDirection = Vector2.Zero;

    //    if (ignoreMap.IsIndexInBounds(cellPos + N) && !ignoreMap[cellPos.X + N.X, cellPos.Y + N.Y] && heatMap.TryGetValue(cellPos + N, out cellValue)) { if (cellValue > highestValue) { highestDirection = N; highestValue = cellValue; highestCell = cellPos + N; averageDirectionTotal += N; } cellCount++; }
    //    if (ignoreMap.IsIndexInBounds(cellPos + S) && !ignoreMap[cellPos.X + S.X, cellPos.Y + S.Y] && heatMap.TryGetValue(cellPos + S, out cellValue)) { if (cellValue > highestValue) { highestDirection = S; highestValue = cellValue; highestCell = cellPos + S; averageDirectionTotal += S; } cellCount++; }
    //    if (ignoreMap.IsIndexInBounds(cellPos + E) && !ignoreMap[cellPos.X + E.X, cellPos.Y + E.Y] && heatMap.TryGetValue(cellPos + E, out cellValue)) { if (cellValue > highestValue) { highestDirection = E; highestValue = cellValue; highestCell = cellPos + E; averageDirectionTotal += E; } cellCount++; }
    //    if (ignoreMap.IsIndexInBounds(cellPos + W) && !ignoreMap[cellPos.X + W.X, cellPos.Y + W.Y] && heatMap.TryGetValue(cellPos + W, out cellValue)) { if (cellValue > highestValue) { highestDirection = W; highestValue = cellValue; highestCell = cellPos + W; averageDirectionTotal += W; } cellCount++; }

    //    if (eightWayMovement)
    //    {
    //        if (ignoreMap.IsIndexInBounds(cellPos + NE) && !ignoreMap[cellPos.X + NE.X, cellPos.Y + NE.Y] && heatMap.TryGetValue(cellPos + NE, out cellValue)) { if (cellValue > highestValue) { highestDirection = NE; highestValue = cellValue; highestCell = cellPos + NE; averageDirectionTotal += NE; } cellCount++; }
    //        if (ignoreMap.IsIndexInBounds(cellPos + NW) && !ignoreMap[cellPos.X + NW.X, cellPos.Y + NW.Y] && heatMap.TryGetValue(cellPos + NW, out cellValue)) { if (cellValue > highestValue) { highestDirection = NW; highestValue = cellValue; highestCell = cellPos + NW; averageDirectionTotal += NW; } cellCount++; }
    //        if (ignoreMap.IsIndexInBounds(cellPos + SE) && !ignoreMap[cellPos.X + SE.X, cellPos.Y + SE.Y] && heatMap.TryGetValue(cellPos + SE, out cellValue)) { if (cellValue > highestValue) { highestDirection = SE; highestValue = cellValue; highestCell = cellPos + SE; averageDirectionTotal += SE; } cellCount++; }
    //        if (ignoreMap.IsIndexInBounds(cellPos + SW) && !ignoreMap[cellPos.X + SW.X, cellPos.Y + SW.Y] && heatMap.TryGetValue(cellPos + SW, out cellValue)) { if (cellValue > highestValue) { highestDirection = SW; highestValue = cellValue; highestCell = cellPos + SW; averageDirectionTotal += SW; } cellCount++; }
    //    }

    //    Debug.Log($"{highestCell}: {highestValue} , {highestDirection}");
    //    return (highestDirection).Normalized();
    //}
    public Vector2 GetHeatGradientTarget(Vector2 pos, float threshold = 0.0f)
    {
        Vector2I cellPos = (pos / CellSize).FloorToInt();

        if (!heatMap.IsIndexInBounds(cellPos))
        {
            return Vector2.Zero;
        }

        var averageDirectionTotal = Vector2.Zero;
        float currentCellValue = heatMap[cellPos.X, cellPos.Y];
        float cellValue = 0;
        float cellCount = 0;
        float highestValue = heatMap[cellPos.X, cellPos.Y];
        Vector2 highestCell = cellPos;
        Vector2 highestDirection = Vector2.Zero;

        if (ignoreMap.IsIndexInBounds(cellPos + N) && !ignoreMap[cellPos.X + N.X, cellPos.Y + N.Y] && heatMap.TryGetValue(cellPos + N, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = N; highestValue = cellValue; highestCell = cellPos + N; averageDirectionTotal += N; } cellCount++; }
        if (ignoreMap.IsIndexInBounds(cellPos + S) && !ignoreMap[cellPos.X + S.X, cellPos.Y + S.Y] && heatMap.TryGetValue(cellPos + S, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = S; highestValue = cellValue; highestCell = cellPos + S; averageDirectionTotal += S; } cellCount++; }
        if (ignoreMap.IsIndexInBounds(cellPos + E) && !ignoreMap[cellPos.X + E.X, cellPos.Y + E.Y] && heatMap.TryGetValue(cellPos + E, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = E; highestValue = cellValue; highestCell = cellPos + E; averageDirectionTotal += E; } cellCount++; }
        if (ignoreMap.IsIndexInBounds(cellPos + W) && !ignoreMap[cellPos.X + W.X, cellPos.Y + W.Y] && heatMap.TryGetValue(cellPos + W, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = W; highestValue = cellValue; highestCell = cellPos + W; averageDirectionTotal += W; } cellCount++; }

        if (eightWayMovement)
        {
            if (ignoreMap.IsIndexInBounds(cellPos + NE) && !ignoreMap[cellPos.X + NE.X, cellPos.Y + NE.Y] && heatMap.TryGetValue(cellPos + NE, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = NE; highestValue = cellValue; highestCell = cellPos + NE; averageDirectionTotal += NE; } cellCount++; }
            if (ignoreMap.IsIndexInBounds(cellPos + NW) && !ignoreMap[cellPos.X + NW.X, cellPos.Y + NW.Y] && heatMap.TryGetValue(cellPos + NW, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = NW; highestValue = cellValue; highestCell = cellPos + NW; averageDirectionTotal += NW; } cellCount++; }
            if (ignoreMap.IsIndexInBounds(cellPos + SE) && !ignoreMap[cellPos.X + SE.X, cellPos.Y + SE.Y] && heatMap.TryGetValue(cellPos + SE, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = SE; highestValue = cellValue; highestCell = cellPos + SE; averageDirectionTotal += SE; } cellCount++; }
            if (ignoreMap.IsIndexInBounds(cellPos + SW) && !ignoreMap[cellPos.X + SW.X, cellPos.Y + SW.Y] && heatMap.TryGetValue(cellPos + SW, out cellValue) && cellValue > threshold) { if (cellValue > highestValue) { highestDirection = SW; highestValue = cellValue; highestCell = cellPos + SW; averageDirectionTotal += SW; } cellCount++; }
        }

        return highestCell * CellSize;
    }


    //public List<(float, Vector2, Vector2I)> GetHeatGradientDirectionList(Vector2 pos)
    //{
    //    var list = new List<(float, Vector2, Vector2I)>();
    //    Vector2I cellPos = (pos / CellSize).FloorToInt();
    //    var cellValue = 0.0f;
    //    if (heatMap.TryGetValue(cellPos, out cellValue)) { list.Add((cellValue, cellPos, Vector2I.Zero)); }
    //    if (heatMap.TryGetValue(cellPos + N, out cellValue)) { list.Add((cellValue, cellPos + N, N)); }
    //    if (heatMap.TryGetValue(cellPos + S, out cellValue)) { list.Add((cellValue, cellPos + S, S)); }
    //    if (heatMap.TryGetValue(cellPos + E, out cellValue)) { list.Add((cellValue, cellPos + E, E)); }
    //    if (heatMap.TryGetValue(cellPos + W, out cellValue)) { list.Add((cellValue, cellPos + W, W)); }

    //    if (eightWayAverage)
    //    {
    //        if (heatMap.TryGetValue(cellPos + NE, out cellValue)) { list.Add((cellValue, cellPos + NE, NE)); }
    //        if (heatMap.TryGetValue(cellPos + NW, out cellValue)) { list.Add((cellValue, cellPos + NW, NW)); }
    //        if (heatMap.TryGetValue(cellPos + SE, out cellValue)) { list.Add((cellValue, cellPos + SE, SE)); }
    //        if (heatMap.TryGetValue(cellPos + SW, out cellValue)) { list.Add((cellValue, cellPos + SW, SW)); }
    //    }

    //    return list;
    //}
    public Vector2I GetCell(Vector2 pos)
    {
        return (pos / CellSize).FloorToInt();
    }
}
