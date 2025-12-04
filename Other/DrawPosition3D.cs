using Godot;
using System;

[Tool]
public partial class DrawPosition3D : Node3D
{
	[Export] bool editorOnly;
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint() || !editorOnly)
		{
			//DebugDraw3D.DrawSphere(this.GlobalPosition, 0.5f, Colors.Red);
			//TODO: Replace DebugDraw3D requirement
		}
	}
}
