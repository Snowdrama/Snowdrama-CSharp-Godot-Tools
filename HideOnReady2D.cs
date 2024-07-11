using Godot;
using System;

public partial class HideOnReady2D : Node2D
{
	public override void _Ready()
	{
		this.Visible = false;
	}
}
