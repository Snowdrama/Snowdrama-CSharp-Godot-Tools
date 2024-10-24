using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class VirtualCameraBrain3D : Camera3D
{
    public static VirtualCameraBrain3D cameraInstance;
    public static List<VirtualCamera3D> cameras;
    public Transform3D cameraTransform = Transform3D.Identity;
    public static Vector3 cameraPosition;
    public static Vector3 cameraSize;
    VirtualCamera3D currentCamera;


    Vector3 currentRotation;
    Vector3 lastRotation;
    float rotX;
    float rotY;
    float rotZ;
    float distRotX;
    float distRotY;
    float distRotZ;

    [Export] float moveSpeed = 5.0f;
    [Export] float rotationSpeed = 5.0f;

    CameraPositionMessage cameraPositionMessage;
    public override void _EnterTree()
    {
        base._EnterTree();
        cameraInstance = this;

        cameraPositionMessage = Messages.Get<CameraPositionMessage>();
    }

    public override void _ExitTree()
    {

        base._ExitTree();
        cameraInstance = null;
        Messages.Return<CameraPositionMessage>();
    }

    public override void _Process(double delta)
    {
        VirtualCameraBrain3D.cameraPosition = this.Position;

        //if the list is null...then there's no VCams
        if (cameras == null)
        {
            return;
        }

        //can't do anything if there's no VCams
        if (cameras.Count == 0)
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
        currentCamera = cameras[0];

        //current one is the highest virtualCameraPriority
        if (currentCamera != cameras[0])
        {
            currentCamera = cameras[0];
        }

        if (currentCamera != null)
        {
            if (currentCamera.lerpToTarget)
            {
                var distanceToTarget = this.GlobalPosition.DistanceTo(currentCamera.Position);
                this.GlobalPosition = this.GlobalPosition.MoveToward(currentCamera.GlobalPosition, (float)delta * distanceToTarget * moveSpeed);
            }
            else
            {
                this.GlobalPosition = currentCamera.GlobalPosition;
            }



            if (currentCamera.enableLookAtTarget)
            {
                this.LookAt(currentCamera.targetToLookAt.GlobalPosition, Vector3.Up);
            }
            else
            {
                this.GlobalRotation = currentCamera.GlobalRotation;
            }
        }

        cameraPositionMessage.Dispatch(this.GlobalPosition);
    }

    public static void RegisterCamera(VirtualCamera3D cam)
    {
        ConfigureCameraList();
        if (cameras != null && !cameras.Contains(cam))
        {
            cameras.Add(cam);
        }
    }
    public static void UnregisterCamera(VirtualCamera3D cam)
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
            cameras = new List<VirtualCamera3D>();
        }
    }
}
