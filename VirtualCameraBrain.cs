using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using static Godot.HttpRequest;

//asks to be the primary camera
public partial class VirtualCameraBrain : Camera2D
{
    public static List<VirtualCamera> cameras;

    VirtualCamera currentCamera;
    [Export] bool smoothScale;

    
    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _ExitTree()
    {

        base._ExitTree();
    }

    public override void _Process(double delta)
    {
        //if the list is null...then there's no VCams
        if(cameras == null)
        {
            return;
        }

        //can't do anything if there's no VCams
        if(cameras.Count == 0)
        {
            return;
        }

        //sort the cameras by their priority
        cameras.Sort((x, y) =>
        {
            if (x.priority > y.priority)
            {
                return 0;
            }
            return 1;
        });

        //current one is the highest priority
        if(currentCamera != cameras[0])
        {
            currentCamera = cameras[0];
        }

        if (currentCamera != null)
        {
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





    public static void RegisterCamera(VirtualCamera cam)
    {
        ConfigureCameraList();
        if (cameras != null && !cameras.Contains(cam))
        {
            cameras.Add(cam);
        }
    }
    public static void UnregisterCamera(VirtualCamera cam)
    {
        ConfigureCameraList();
        if (cameras != null && cameras.Contains(cam))
        {
            cameras.Remove(cam);
        }

    }
    private static void ConfigureCameraList()
    {
        if (cameras == null)
        {
            cameras = new List<VirtualCamera>();
        }
    }
}
