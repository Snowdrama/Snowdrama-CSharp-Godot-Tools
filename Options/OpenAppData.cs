using Godot;
using System;

public partial class OpenAppData : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.Pressed += OpenAppData_Pressed;
	}

    private void OpenAppData_Pressed()
    {
        DirAccess.Open("user://");
    }
}
