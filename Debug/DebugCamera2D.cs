using Godot;
using System;

public partial class DebugCamera2D : Camera2D
{
    [Export]
    float speed = 5;

    [Export]
    float zoomSpeed = 0.1f;
    float zoom = 1.0f;
    public override void _Ready()
    {
        this.PositionSmoothingEnabled = true;
        this.PositionSmoothingSpeed = 10.0f;
    }
    public override void _Process(double delta)
    {
        var direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
        this.Position += direction * speed;

        var zoomAxis = Input.GetAxis("ZoomOut", "ZoomIn");
        zoom += zoomAxis * zoomSpeed;
        zoom = Mathf.Clamp(zoom, 0.01f, 1000);
        this.Zoom = new Vector2(zoom, zoom);
    }
}
