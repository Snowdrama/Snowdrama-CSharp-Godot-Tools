using Godot;
using Snowdrama.Spring;
using System;

public partial class SubViewportSizeController : SubViewport
{
	[Export] bool scaleToWindowResolution;

	[Export] Vector2 sizeScale = new Vector2(1.0f, 1.0f);


	Vector2I windowSize;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(windowSize != DisplayServer.WindowGetSize())
		{
			windowSize = DisplayServer.WindowGetSize();
			this.Size = new Vector2I(Mathf.RoundToInt(windowSize.X * sizeScale.X), Mathf.RoundToInt(windowSize.Y * sizeScale.Y));
		}
    }
}
