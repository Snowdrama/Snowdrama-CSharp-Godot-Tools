using Godot;
using Snowdrama.Core;
using System;
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

public partial class HeatDiffusionFill : Node2D
{
    [Export(PropertyHint.Range, "0.5, 2.0")] float constantFalloff = 0.75f;
    float[,] heatGenerators = new float[100, 100];
    float[,] heatMap = new float[100, 100];
    bool[,] ignoreMap = new bool[100, 100];
    [Export] Vector2I size = new Vector2I(100, 100);
    public void SetHeatGenerator(Vector2I position, float amount)
    {
        heatGenerators[position.X, position.Y] = amount;
        ignoreMap[position.X, position.Y] = true;
    }
    public void SetIgnore(Vector2I position, bool ignore)
    {
        ignoreMap[position.X, position.Y] = ignore;
    }

    public override void _Ready()
    {
        base._Ready();
        heatGenerators = new float[size.X, size.Y];
        heatMap = new float[size.X, size.Y];
        ignoreMap = new bool[size.X, size.Y];

        for (int i = 0; i < 4; i++)
        {
            var randomX = RandomAndNoise.RandomRange(0, heatMap.GetLength(0));
            var randomY = RandomAndNoise.RandomRange(0, heatMap.GetLength(1));
            SetHeatGenerator(new Vector2I(randomX, randomY), 1.0f);
        }

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
        if (Input.IsActionPressed("ui_select"))
        {
            sw.Restart();
            UpdateTick();
            sw.Stop();
            Debug.Log($"Stopwatch: {sw.ElapsedMilliseconds}");
        }

        QueueRedraw();
    }

    public void UpdateTick()
    {
        for (int y = 0; y < heatMap.GetLength(1); y++)
        {
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                if (heatGenerators[x, y] > 0.1f)
                {
                    heatMap[x, y] = heatGenerators[x, y];
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

        for (int y = 0; y < heatMap.GetLength(1); y++)
        {
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                if (heatGenerators[x, y] > 0.1f)
                {
                    heatMap[x, y] = heatGenerators[x, y];
                }
            }
        }
    }

    [Export] float rectSize = 25;
    [Export] float rectOffset = 30;

    public override void _Draw()
    {
        base._Draw();
        for (int y = 0; y < heatMap.GetLength(1); y++)
        {
            for (int x = 0; x < heatMap.GetLength(0); x++)
            {
                DrawRect(
                    new Rect2(
                        x * rectOffset,
                        y * rectOffset,
                        rectSize,
                        rectSize
                    ),
                    new Color(0, 0, heatMap[x, y])
                );

                //DrawString(
                //    _defaultFont,
                //    new Vector2(
                //        x * rectOffset,
                //        (y * rectOffset) + 12
                //    ),
                //    $"{heatMap[x, y]:F4}"
                //);
            }
        }
    }
    private Font _defaultFont = ThemeDB.FallbackFont;
    private float[,] AverageCells(Vector2I pos, float[,] cells, bool[,] ignore, float constant = 1.0f)
    {
        var totalValue = cells[pos.X, pos.Y];
        var cellValue = cells[pos.X, pos.Y];
        float cellCount = 1;
        if (!ignore[pos.X, pos.Y])
        {
            if (cells.TryGetValue(pos + new Vector2I(0, 1), out cellValue)) { totalValue += cellValue; cellCount++; }
            if (cells.TryGetValue(pos + new Vector2I(0, -1), out cellValue)) { totalValue += cellValue; cellCount++; }
            if (cells.TryGetValue(pos + new Vector2I(1, 0), out cellValue)) { totalValue += cellValue; cellCount++; }
            if (cells.TryGetValue(pos + new Vector2I(-1, 0), out cellValue)) { totalValue += cellValue; cellCount++; }
            var test = (totalValue * constant) / cellCount;
            cells[pos.X, pos.Y] = test;
        }
        return cells;
    }
}
