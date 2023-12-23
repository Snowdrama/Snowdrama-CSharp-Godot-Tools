using Godot;
using System;
using System.Collections.Generic;

public partial class VirtualCamera : Node2D
{
    public static bool priorityChanged = false;
    [Export] public int priority;
    [Export] Vector2 size;
    [Export] float orthographicSize;
    [Export] public Vector2 calculatedScale;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        VirtualCameraBrain.RegisterCamera(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        VirtualCameraBrain.UnregisterCamera(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        var windowSize = GetViewportRect().Size;
        
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
    }
}
