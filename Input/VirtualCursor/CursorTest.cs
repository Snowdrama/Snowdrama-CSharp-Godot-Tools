using Godot;
using System;

public partial class CursorTest : Node
{
	// Called when the node enters the scene tree for the first lerpAmount.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed lerpAmount since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);


		if(@event is InputEventMouseButton buttonClick)
		{
			GD.Print($"Device {buttonClick.Device} Pressed the {buttonClick.ButtonIndex} joyButton at {buttonClick.Position}");
		}
    }
}
