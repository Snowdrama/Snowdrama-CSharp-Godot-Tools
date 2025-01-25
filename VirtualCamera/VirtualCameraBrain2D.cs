using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using static Godot.HttpRequest;

//asks to be the primary camera
public partial class VirtualCameraBrain2D : Camera2D
{
    public static VirtualCameraBrain2D cameraInstance;
    public static List<VirtualCamera2D> cameras;
    public Transform2D cameraTransform = Transform2D.Identity;
    public static Vector2 cameraPosition;
    public static Vector2 cameraSize;

    VirtualCamera2D currentCamera;
    [ExportGroup("Zoom")]
    [Export] bool smoothScale;

    [ExportGroup("Position")]
    [Export] bool LerpPosition;
    [Export] double LerpPositionLinearSpeed = 10.0;
    [Export] bool LerpPositionByDistance;
    [Export] double LerpPositionDistanceSpeed = 10.0;

    [Export] double maxSpeed = 100;


    [ExportGroup("Rotation")]
    [Export] bool LerpRotation;
    [Export] double LerpRotationSpeed = 10.0;

    public override void _EnterTree()
    {
        base._EnterTree();
        cameraInstance = this;



    }

    public override void _ExitTree()
    {

        base._ExitTree();
        cameraInstance = null;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        VirtualCameraBrain2D.cameraPosition = this.Position;
        cameraTransform = GetCanvasTransform();

        //if the list is null...then there's no VCams
        if (cameras == null)
        {
            return;
        }

        //can't do anything if there's no VCams
        if(cameras.Count == 0)
        {
            return;
        }
        
        //sort the cameras by their virtualCameraPriority
        cameras.Sort((x, y) =>
        {
            if (x.virtualCameraPriority > y.virtualCameraPriority)
            {
                return -1;
            }
            if (x.virtualCameraPriority < y.virtualCameraPriority)
            {
                return 1;
            }
            return 0;
        });

        //current one is the highest virtualCameraPriority
        if (currentCamera != cameras[0])
        {
            currentCamera = cameras[0];
        }

        if (currentCamera != null)
        {
            this.PositionSmoothingEnabled = currentCamera.PositionSmoothingEnabled;
            this.PositionSmoothingSpeed = currentCamera.PositionSmoothingSpeed;
            if (LerpPosition)
            {
                if (LerpPositionByDistance)
                {
                    var distance = this.Position.DistanceTo(currentCamera.GlobalPosition);
                    var currentDelta = (delta * LerpPositionLinearSpeed) + (delta * LerpPositionDistanceSpeed * distance);
                    var clampedDelta = Mathf.Clamp(currentDelta, 0, maxSpeed);
                    this.Position = this.Position.MoveToward(currentCamera.GlobalPosition, (float)clampedDelta);
                }
                else
                {
                    var currentDelta = (float)(delta * LerpPositionLinearSpeed);
                    var clampedDelta = Mathf.Clamp(currentDelta, 0, maxSpeed);
                    this.Position = this.Position.MoveToward(currentCamera.GlobalPosition, (float)clampedDelta);
                }
            }
            else
            {
                this.Position = currentCamera.GlobalPosition;
            }

            if (LerpRotation)
            {
                this.Rotation = this.Rotation.MoveTowards(currentCamera.GlobalRotation,(float)(delta * LerpRotationSpeed));
            }
            else
            {
                this.Rotation = currentCamera.GlobalRotation;
            }

            if (smoothScale && currentCamera.cameraZoomLevel.Length() > 0.1f)
            {
                this.Zoom = Vector2Extensions.Lerp(this.Zoom, currentCamera.cameraZoomLevel, (float)delta);
            }
            else
            {
                this.Zoom = currentCamera.cameraZoomLevel;
            }
        }
    }

    public static void RegisterCamera(VirtualCamera2D cam)
    {
        ConfigureCameraList();
        if (cameras != null && !cameras.Contains(cam))
        {
            cameras.Add(cam);
        }
    }
    public static void UnregisterCamera(VirtualCamera2D cam)
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
            cameras = new List<VirtualCamera2D>();
        }
    }
}
