using Godot;
using System;

public partial class DebugCamera2D : VirtualCamera2D
{
    [Export]
    float speed = 5;

    [Export]
    float zoomSpeed = 0.1f;
    float zoom = 0.0f;


    [Export]
    float zoomMax = 5;
    [Export]
    float zoomMin = 0.5f;
    public override void _Ready()
    {
        this.PositionSmoothingEnabled = true;
        this.PositionSmoothingSpeed = 10.0f;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        var direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
        this.Position += direction * speed;

        var zoomAxis = Input.GetAxis("ZoomOut", "ZoomIn");
        zoom += zoomAxis * zoomSpeed * (float)delta;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        this.relativeZoom = new Vector2(zoom, zoom);
    }
}
