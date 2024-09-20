using Godot;
using System;

public partial class DebugCamera3D : Node3D
{
    [Export]
    public Camera3D cam;

    [Export]
    float speed = 5;

    [Export]
    float zoomSpeed = 0.1f;
    float zoom = 1.0f;

    [Export]
    float lerpSpeed = 1.0f;

    float pitch = 0;
	float yaw = 0;

    Vector2 moveDirection2D;
    Vector3 moveDirection;
    Vector2 turnDirection;
    Vector2 mouseDelta;
    float verticalVelocity;

    [ExportCategory("Input Acceleration")]
    [Export] float gamepadAcceleration = 2.5f;
    [Export] float mouseSensitivity = 0.1f;
    [Export] float mouseAcceleration = 1;


    Vector3 targetPosition;

    public override void _Ready()
    {
    }
    public override void _Process(double delta)
    {
        if (mouseDelta.LengthSquared() >= 0.1f)
        {
            yaw -= mouseDelta.X * mouseAcceleration;
            pitch -= mouseDelta.Y * mouseAcceleration;
            mouseDelta.X = 0;
            mouseDelta.Y = 0;
        }

        if (turnDirection.Length() >= 0.1f)
        {
            yaw -= turnDirection.X * gamepadAcceleration;
            pitch -= turnDirection.Y * gamepadAcceleration;
        }

        this.Rotation = new Vector3(0,Mathf.DegToRad((float)yaw),0);
        cam.Rotation = new Vector3(Mathf.DegToRad((float)pitch), 0, 0);

        var rotatedMoveDirection = moveDirection.Rotated(Vector3.Up, Mathf.DegToRad((float)yaw)) ;

        var direction3D = new Vector3(rotatedMoveDirection.X, verticalVelocity, rotatedMoveDirection.Z);

        targetPosition += direction3D * speed * (float)delta; 
        var distanceToTarget = this.Position.DistanceTo(targetPosition);
        this.Position = Vector3Extensions.MoveTowards(this.Position, targetPosition, lerpSpeed  * distanceToTarget * (float)delta);
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseButton eventMouseButton)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            mouseDelta += eventMouseMotion.Relative * mouseSensitivity;
        }

        if (@event.IsAction("TurnLeft") || @event.IsAction("TurnRight") || @event.IsAction("TurnDown") || @event.IsAction("TurnUp"))
        {
            turnDirection = Input.GetVector("TurnLeft", "TurnRight", "TurnDown", "TurnUp");
        }

        if(@event.IsAction("ZoomIn") || @event.IsAction("ZoomOut"))
        {
            verticalVelocity = Input.GetAxis("ZoomOut", "ZoomIn"); 
        }


        if (@event.IsAction("MoveLeft") || @event.IsAction("MoveRight") || @event.IsAction("MoveDown") || @event.IsAction("MoveUp"))
        {
            moveDirection2D = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
            moveDirection.X = moveDirection2D.X;
            moveDirection.Z = moveDirection2D.Y;
        }


        if(@event is InputEventKey key)
        {
            if(key.Keycode is Key.Escape)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }
    }
}
