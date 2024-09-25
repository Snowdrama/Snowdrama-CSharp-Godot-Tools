using Godot;
using System;




[Tool, GlobalClass]
public partial class PathCSGLineRenderer : Node3D
{
    [Export] float line_radius = 0.1f;
    [Export] int line_resolution = 180;

	[Export] CsgPolygon3D polygon;
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if(polygon == null)
        {
            return;
        }

		var circle = new Vector2[line_resolution];

		for (int i = 0; i < line_resolution; i++)
        {
            var x = line_radius * Mathf.Sin(Mathf.Pi * 2 * i / line_resolution);
            var y = line_radius * Mathf.Cos(Mathf.Pi * 2 * i / line_resolution);
            circle[i] = new Vector2(x, y);
        }
        polygon.Polygon = circle;
    }
}
