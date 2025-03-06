using Godot;
using System;

public partial class DebugCamera2D : VirtualCamera2D
{
    [Export]
    float speed = 5;

    [Export]
    float zoomSpeed = 0.1f;
    float zoom = 1.0f;


    [Export]
    float zoomMax = 5;
    [Export]
    float zoomMin = 0.5f;

    [ExportGroup("Game Input Names")]
    [Export] string Left = "MoveCameraLeft";
    [Export] string Right = "MoveCameraRight";
    [Export] string Up = "MoveCameraUp";
    [Export] string Down = "MoveCameraDown";
    [Export] string ZoomIn = "CameraZoomIn";
    [Export] string ZoomOut = "CameraZoomOut";
    public override void _Ready()
    {
        this.PositionSmoothingEnabled = true;
        this.PositionSmoothingSpeed = 10.0f;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        var direction = Input.GetVector(Left, Right, Up, Down);
        this.Position += direction * speed;

        var zoomAxis = Input.GetAxis(ZoomOut, ZoomIn);
        zoom += zoomAxis * zoomSpeed * (float)delta;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        this.relativeZoom = new Vector2(zoom, zoom);
    }
}
