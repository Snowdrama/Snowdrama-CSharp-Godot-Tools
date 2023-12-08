using Godot;
using System;
using System.Collections.Generic;

public partial class VirtualCamera : Node2D
{
    public static List<VirtualCamera> cameras = new List<VirtualCamera>();
    public static bool priorityChanged = false;
    [Export]  public int priority;
    private int oldPriority;
    [Export] Vector2 size;
    [Export] float orthographicSize;
    [Export] public Vector2 calculatedScale;

    public override void _Ready()
    {
        base._Ready();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
        base._EnterTree();
        cameras.Add(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        cameras.Remove(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        var windowSize = GetViewportRect().Size;

        //GD.Print($"windowSize {windowSize}");

        calculatedScale.X = Mathf.FloorToInt(windowSize.X / size.X);
        calculatedScale.Y = Mathf.FloorToInt(windowSize.Y / size.Y);

        if (calculatedScale.X > calculatedScale.Y)
        {
            calculatedScale.X = calculatedScale.Y;
        }
        else
        {
            calculatedScale.Y = calculatedScale.X;
        }


        if(oldPriority != priority)
        {
            priorityChanged = true;
            oldPriority = priority;
        }
        //GD.Print($"calculatedScale.X: {windowSize.X} / {Size.X} = {windowSize.X / Size.X}");
        //GD.Print($"calculatedScale.Y: {windowSize.Y} / {Size.Y} = {windowSize.Y / Size.Y}");
        //GD.Print($"calculatedScale {calculatedScale}");
    }
}
