using Godot;
using System;

public partial class Debug3DCharacterController : CharacterBody3D
{
    [Export] Camera3D cam;

    [ExportCategory("Physics")]
    [Export] float jumpHeight = 5;
    [Export] float jumpTime = 1.0f;
    [Export] float currentSpeed = 5;
    [Export] float walkingSpeed = 2.5f;
    [Export] float sprintSpeed = 5.0f;
    [Export] float accelerationSpeed = 100;
    [Export] float decelerationSpeed = 15;

    double gravity = 10;
    double jumpForce = 5;

    Vector3 rotation;

    float pitch = 0;
    float yaw = 0;

    [ExportCategory("Input")]
    [Export] Vector2 mouseDelta;
    [Export] Vector2 turnDirection;
    [Export] Vector2 moveDirection2D;
    [Export] Vector3 moveDirection;
    [Export] Vector3 horizontalVelocity;
    [Export] Vector3 localVelocity;

    [Export] double verticalVelocity;

    [ExportCategory("Input Acceleration")]
    [Export] float gamepadAcceleration = 2.5f;
    [Export] float mouseSensitivity = 0.1f;
    [Export] float mouseAcceleration = 1;

    public override void _Ready()
    {
    }

    int jumpCount = 2;
    int jumpCountMax = 2;
    bool grounded;
    bool jump;
    bool sprint;
    bool sprintKeyPressed;
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        gravity = PhysicsTools.DeriveGravity(jumpHeight, jumpTime / 2);
        jumpForce = PhysicsTools.DeriveInitialVelocity(jumpHeight, jumpTime / 2, -gravity);

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


        if ((grounded || jumpCount > 0) && jump)
        {
            jumpCount--;
            verticalVelocity = jumpForce;
            grounded = false;
            jump = false;
        }

        if (sprintKeyPressed)
        {
            //sprint is true if the Key is pressed regardles of maxFlyingSpeed
            sprint = true;
        }
        else if (!sprintKeyPressed && moveDirection.LengthSquared() < 0.1f)
        {
            //if we aren't holding the sprint Key AND we are not moving
            sprint = false;
        }

        if (sprint)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkingSpeed;
        }

        yaw %= 360;
        pitch = Mathf.Clamp(pitch, -60, 60);

        this.Rotation = new Vector3(0, Mathf.DegToRad((float)yaw), 0);
        cam.Rotation = new Vector3(Mathf.DegToRad((float)pitch), 0, 0);


        horizontalVelocity = moveDirection.Rotated(Vector3.Up, Mathf.DegToRad((float)yaw)) * currentSpeed;


        if (horizontalVelocity.LengthSquared() > 0.5f)
        {
            horizontalVelocity = horizontalVelocity.MoveToward(horizontalVelocity, (float)delta * accelerationSpeed);
        }
        else
        {
            horizontalVelocity = horizontalVelocity.MoveToward(horizontalVelocity, (float)delta * decelerationSpeed);
        }

        localVelocity.X = horizontalVelocity.X;
        localVelocity.Z = horizontalVelocity.Z;

        localVelocity.Y = (float)verticalVelocity;

        verticalVelocity -= gravity * delta;
        verticalVelocity = Mathf.Clamp(verticalVelocity, -gravity, 1000);

        this.Velocity = localVelocity;
        this.MoveAndSlide();

        for (int i = 0; i < this.GetSlideCollisionCount(); i++)
        {
            var collision = this.GetSlideCollision(i);
        }

        if (verticalVelocity < 0 && this.IsOnFloor())
        {
            jumpCount = jumpCountMax;
            verticalVelocity = 0;
            grounded = true;
        }
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

        if (@event.IsActionPressed("Jump"))
        {
            jump = true;
        }
        if (@event.IsActionReleased("Jump"))
        {
            jump = false;
        }

        if (@event.IsActionPressed("Sprint"))
        {
            sprintKeyPressed = true;
        }
        if (@event.IsActionReleased("Sprint"))
        {
            sprintKeyPressed = false;
        }

        if (@event.IsAction("MoveLeft") || @event.IsAction("MoveRight") || @event.IsAction("MoveDown") || @event.IsAction("MoveUp"))
        {
            moveDirection2D = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
            moveDirection.X = moveDirection2D.X;
            moveDirection.Z = moveDirection2D.Y;
        }


        if (@event is InputEventKey key)
        {
            if (key.Keycode is Key.Escape)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }
    }
}