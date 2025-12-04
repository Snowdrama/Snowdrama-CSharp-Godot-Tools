using Godot;
using System;

public partial class HideOnReady3D : Node3D
{
	public override void _Ready()
	{
		this.Visible = false;
	}
}
