using Godot;
using System;
using System.Collections.Generic;

[Tool, GlobalClass]
public partial class DistributeInBox2D : Node2D
{
    public enum DistributeInBox2DType
    {
        None = 0,
        Grid = 1,
        Box = 2,
    }
    [Export] private DistributeInBox2DType type;
    [Export] private Vector2I size;
    [Export] private Vector2 spacing;
    [Export] private bool runInPlayMode = true;
    [Export] private bool runInEditor = true;

    [ExportCategory("Debug")]
    [Export] private bool runNow = true;

    private List<Vector2I> positions;
    private Vector2 centeringOffset;
    public override void _Ready()
    {
        base._Ready();
        if (Engine.IsEditorHint() && runInEditor)
        {
            UpdateDistribution();
        }

        if (runInPlayMode)
        {
            UpdateDistribution();
        }
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (runNow)
        {
            runNow = false;
            if (Engine.IsEditorHint() && runInEditor)
            {
                UpdateDistribution();
            }

            if (runInPlayMode)
            {
                UpdateDistribution();
            }
        }
    }

    public void UpdateDistribution()
    {
        CollectChildren();

        centeringOffset = (-(size * spacing) * 0.5f) + (spacing * 0.5f);
        switch (type)
        {
            case DistributeInBox2DType.None:
                break;
            case DistributeInBox2DType.Grid:
                positions = CreateSquareGrid(size);
                break;
            case DistributeInBox2DType.Box:
                positions = CreateSquareRing(size);
                break;
            default:
                break;
        }
        for (int i = 0; i < children.Count; i++)
        {
            Debug.Green($"Updating Position: {positions[i] * spacing}");
            children[i].Position = centeringOffset + (positions[i] * spacing);
        }
    }

    private List<Node2D> children = new List<Node2D>();
    public void CollectChildren()
    {
        children.Clear();
        Debug.Red($"Getting Children {this.GetChildCount()}");
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            var child = this.GetChild(i);
            if (child != null && child is Node2D dt)
            {
                children.Add(dt);
            }
        }
    }

    /// <summary>
    /// Creates a full grid of points covering a rectangle of given size.
    /// </summary>
    public List<Vector2I> CreateSquareGrid(Vector2I size)
    {
        var result = new List<Vector2I>();

        int width = size.X;
        int height = size.Y;

        if (width <= 0 || height <= 0)
            return result;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result.Add(new Vector2I(x, y));
            }
        }

        return result;
    }

    public List<Vector2I> CreateSquareRing(Vector2I size)
    {
        var result = new List<Vector2I>();

        int width = size.X;
        int height = size.Y;

        if (width <= 0 || height <= 0)
            return result;

        // Top row (left → right)
        for (int x = 0; x < width; x++)
            result.Add(new Vector2I(x, 0));

        // Right column (top → bottom, skipping the first corner)
        for (int y = 1; y < height; y++)
            result.Add(new Vector2I(width - 1, y));

        // Bottom row (right → left, skipping the first corner)
        for (int x = width - 2; x >= 0; x--)
            result.Add(new Vector2I(x, height - 1));

        // Left column (bottom → top, skipping the first and last corners)
        for (int y = height - 2; y > 0; y--)
            result.Add(new Vector2I(0, y));

        Debug.Red($"Created Ring Of Size {size} With {result.Count} Positions");
        return result;
    }
}
