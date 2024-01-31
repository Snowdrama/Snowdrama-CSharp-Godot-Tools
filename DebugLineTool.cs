using Godot;
using System;
using System.Collections.Generic;

public partial class DebugLineTool : Node
{
	public class DebugLine
	{
		public float timeToClear = 10;
	}
	Dictionary<string, DebugLine> lines = new Dictionary<string, DebugLine>();
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}
