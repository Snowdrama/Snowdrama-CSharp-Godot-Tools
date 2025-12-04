using Godot;
using System;

[Tool, GlobalClass]
public partial class DebugCamera2D : VirtualCamera2D
{
    [Export]
    private float speed = 5;

    [Export]
    private float zoomSpeed = 0.1f;
    private float zoom = 1.0f;


    [Export]
    private float zoomMax = 5;
    [Export]
    private float zoomMin = 0.5f;

    [ExportGroup("Game Input Names")]
    [Export] private string Left = "MoveCameraLeft";
    [Export] private string Right = "MoveCameraRight";
    [Export] private string Up = "MoveCameraUp";
    [Export] private string Down = "MoveCameraDown";
    [Export] private string ZoomIn = "ZoomCameraIn";
    [Export] private string ZoomOut = "ZoomCameraOut";
    public override void _Ready()
    {
        base._Ready();
        this.PositionSmoothingEnabled = true;
        this.PositionSmoothingSpeed = 10.0f;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Engine.IsEditorHint())
        {
            return;
        }
        var direction = Input.GetVector(Left, Right, Up, Down);
        this.Position += direction * speed;

        var zoomAxis = Input.GetAxis(ZoomOut, ZoomIn);
        zoom += zoomAxis * zoomSpeed * (float)delta;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        this.relativeZoom = new Vector2(zoom, zoom);
    }
}
