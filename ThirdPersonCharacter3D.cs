using Godot;
using ImGuiNET;
using System;

public partial class ThirdPersonCharacter3D : Node
{
    [Export] CharacterBody3D body;
    [Export] AnimationTree animationTree;
    [Export] float moveSpeed = 10.0f;
    
    
    [Export] VirtualCamera3D camera;
    [Export] Vector3 cameraDirection;
    [Export] float cameraDistance = 5.0f;
    [Export] Vector2 yClampMinMax = new Vector2(-60, 20);

    [Export] float playerHeight = 2.0f;


    System.Numerics.Vector2 cameraSpeed = new System.Numerics.Vector2(200, 50);
    System.Numerics.Vector2 cameraSpeedMouse = new System.Numerics.Vector2(100, 25);


    [Export] Node3D yawNode;
    [Export] Node3D pitchNode;

    float yaw = 0;
    float pitch = 0;

    Vector2 mouseDelta;

    [ExportCategory("Animation")]
    [Export] PlayerAnimationController animationController;
    public override void _PhysicsProcess(double delta)
    {
        if (!IsMultiplayerAuthority())
        {
            return;
        }
        base._PhysicsProcess(delta);

        var moveInput = Input.GetVector("left", "right", "up", "down");
        var moveInput3D = new Vector3(moveInput.X, 0, moveInput.Y);
        var rotatedMoveInput = moveInput3D.Rotated(Vector3.Up, Mathf.DegToRad(yaw));

        if(rotatedMoveInput.LengthSquared() > 0.1f)
        {
            animationController.SetTargetRotation(-rotatedMoveInput.SignedAngleTo(Vector3.Right, Vector3.Up));
        }

        body.Velocity = rotatedMoveInput * moveSpeed;
        body.MoveAndSlide();
    }

    public override void _Process(double delta)
    {
        if (!this.IsMultiplayerAuthority())
        {
            if (camera != null)
            {
                camera.MarkAsInactive();
            }
            return;
        }
        camera.MarkAsActive();

        base._Process(delta);
        var cameraInput = Input.GetVector("look_left", "look_right", "look_up", "look_down");

        //var rootPos = animationTree.GetRootMotionPosition();
        //var rootRot = animationTree.GetRootMotionRotation();
        //var currentPos = (animationTree.GetRootMotionPositionAccumulator().Inverse() * body.GetQuaternion());


        var invertX = Options.GetBool("InvertX", false);
        var invertY = Options.GetBool("InvertY", true);
        var xSensitivity = Options.GetFloat("XSensitivity", 0.5f);
        var ySensitivity = Options.GetFloat("YSensitivity", 0.5f);
        var xMouseSensitivity = Options.GetFloat("XMouseSensitivity", 0.5f);
        var yMouseSensitivity = Options.GetFloat("YMouseSensitivity", 0.5f);

        if (mouseDelta.LengthSquared() > 0.1f)
        {
            yaw += mouseDelta.X * (float)delta * cameraSpeedMouse.X * xMouseSensitivity;
            pitch += mouseDelta.Y * (float)delta * cameraSpeedMouse.Y * yMouseSensitivity;
            mouseDelta = Vector2.Zero;
        }

        yaw += cameraInput.X * (float)delta * cameraSpeed.X * xSensitivity * (invertX?-1.0f:1.0f);
        pitch += cameraInput.Y * (float)delta * cameraSpeed.Y * ySensitivity * (invertY?-1.0f:1.0f);
        yaw = yaw.WrapClamp(0.0f, 360.0f);
        pitch = pitch.Clamp(yClampMinMax.X, yClampMinMax.Y);

        cameraDirection = Vector3.Back.Rotated(Vector3.Right, Mathf.DegToRad(pitch)).Rotated(Vector3.Up, Mathf.DegToRad(yaw));
        camera.GlobalPosition = (body.GlobalPosition + new Vector3(0, playerHeight, 0)) + (cameraDirection * cameraDistance);
        camera.LookAt(body.GlobalPosition + new Vector3(0, playerHeight, 0));


        if (MultiplayerGlobals.ShowDebug)
        {
            ImGui.Begin("Player");
            ImGui.SliderFloat2("", ref cameraSpeed, 0.0f, 400.0f);
            ImGui.SliderFloat2("", ref cameraSpeedMouse, 0.0f, 200.0f);
            ImGui.SliderFloat("Move Speed: ", ref moveSpeed, 0.0f, 50.0f);
            ImGui.End();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event != null)
        {
            if(@event is InputEventMouseMotion motion)
            {
                mouseDelta += motion.ScreenRelative;
            }
        }
    }
}
