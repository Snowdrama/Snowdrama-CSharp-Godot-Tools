using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

//asks to be the primary camera
public class CameraPriorityMessage : AMessage<VirtualCamera> { }

public partial class VirtualCameraBrain : Camera2D
{
    VirtualCamera currentCamera;
    [Export] bool smoothScale;


    CameraPriorityMessage CameraPriorityMessage;
    public override void _EnterTree()
    {
        base._EnterTree();
        CameraPriorityMessage = Messages.Get<CameraPriorityMessage>();
        CameraPriorityMessage.AddListener(SetCurrentCamera);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        CameraPriorityMessage.RemoveListener(SetCurrentCamera);

    }

    public void SetCurrentCamera(VirtualCamera newCamera)
    {
        currentCamera = newCamera;
    }

	public override void _Process(double delta)
    {
        if (currentCamera != null)
        {
            //GD.Print($"Updating Camera to: {currentCamera.Offset}");
            this.Position = currentCamera.GlobalPosition;
            this.Rotation = currentCamera.Rotation;
            if (smoothScale && currentCamera.calculatedScale.Length() > 0.1f)
            {
                this.Zoom = Vector2Extensions.Lerp(this.Zoom, currentCamera.calculatedScale, (float)delta);
            }
            else
            {
                this.Zoom = currentCamera.calculatedScale;
            }
        }
    }
}
