using Godot;
using Snowdrama.Spring;
using System.Collections.Generic;

//asks to be the primary camera
[GlobalClass]
public partial class VirtualCameraBrain2D : Camera2D
{
    public static VirtualCameraBrain2D? cameraInstance;
    public static List<VirtualCamera2D> cameras = new List<VirtualCamera2D>();
    public Transform2D cameraTransform = Transform2D.Identity;
    public static Vector2 cameraPosition;
    public static Vector2 cameraSize;

    private VirtualCamera2D? currentCamera;
    [ExportGroup("Zoom")]
    [Export] private bool SmoothScalingEnabled;

    [ExportGroup("Position")]
    [Export] private bool LerpPosition;
    [Export] private double LerpPositionLinearSpeed = 10.0;
    [Export] private bool LerpPositionByDistance;
    [Export] private double LerpPositionDistanceSpeed = 10.0;

    [Export] private double maxSpeed = 100;


    [ExportGroup("Rotation")]
    [Export] private bool LerpRotation;
    [Export] private double LerpRotationSpeed = 10.0;

    private bool isShakingScreen = false;
    private float screenShakeTime = 0.0f;
    private float screenShakeIntensity = 1.0f;
    private Spring2D screenShakeSpring;
    private VirtualCameraBrain2D()
    {
        screenShakeSpring = new Spring2D();
        ScreenShake = Messages.Get<ScreenShakeMessage2D>();
        ScreenShake.AddListener(ShakeScreen);
    }
    ~VirtualCameraBrain2D()
    {
        ScreenShake.RemoveListener(ShakeScreen);
        Messages.Return<ScreenShakeMessage2D>();
    }

    public override void _Ready()
    {
        base._Ready();
    }
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
        VirtualCameraBrain2D.cameraPosition = this.GlobalPosition;
        cameraTransform = GetCanvasTransform();

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

        if (isShakingScreen)
        {
            screenShakeSpring.Velocity = Vector2Extensions.RandomDirection() * screenShakeIntensity;
            if (screenShakeTime > 0)
            {
                screenShakeTime -= (float)delta;
            }
            else
            {
                screenShakeTime = 0;
                screenShakeIntensity = 0;
                isShakingScreen = false;
            }
        }
        screenShakeSpring.Update(delta);

        //sort the cameras by their virtualCameraPriority
        cameras.Sort((x, y) =>
        {
            if (x.forceActive)
            {
                return -1;
            }
            if (y.forceActive)
            {
                return 1;
            }
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
            this.SmoothScalingEnabled = currentCamera.ScaleSmoothingEnabled;
            this.PositionSmoothingEnabled = currentCamera.PositionSmoothingEnabled;
            this.PositionSmoothingSpeed = currentCamera.PositionSmoothingSpeed;
            if (LerpPosition)
            {
                if (LerpPositionByDistance)
                {
                    var distance = this.GlobalPosition.DistanceTo(currentCamera.GlobalPosition);
                    var currentDelta = (delta * LerpPositionLinearSpeed) + (delta * LerpPositionDistanceSpeed * distance);
                    var clampedDelta = Mathf.Clamp(currentDelta, 0, maxSpeed);
                    this.GlobalPosition = this.GlobalPosition.MoveToward(currentCamera.GlobalPosition, (float)clampedDelta);
                }
                else
                {
                    var currentDelta = (float)(delta * LerpPositionLinearSpeed);
                    var clampedDelta = Mathf.Clamp(currentDelta, 0, maxSpeed);
                    this.GlobalPosition = this.GlobalPosition.MoveToward(currentCamera.GlobalPosition, (float)clampedDelta);
                }
                this.GlobalPosition += screenShakeSpring.Value;
            }
            else
            {
                this.GlobalPosition = currentCamera.GlobalPosition + screenShakeSpring.Value;
            }


            this.IgnoreRotation = currentCamera.IgnoreRotation;

            if (LerpRotation)
            {
                this.Rotation = this.Rotation.MoveTowards(currentCamera.GlobalRotation, (float)(delta * LerpRotationSpeed));
            }
            else
            {
                this.Rotation = currentCamera.GlobalRotation;
            }

            if (SmoothScalingEnabled && currentCamera.cameraZoomLevel.Length() > 0.1f)
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

    private ScreenShakeMessage2D ScreenShake;
    private void ShakeScreen(float intensity, float duration = 0.1f, Vector2 biasDirection = new Vector2())
    {
        //Debug.Log($"Shaking Screen! intensity[{intensity}] duration[{duration}], biasDirection[{biasDirection}]");
        isShakingScreen = true;
        screenShakeIntensity = Mathf.Max(screenShakeIntensity, intensity);
        screenShakeTime = Mathf.Max(screenShakeTime, duration);
    }
}

public class ScreenShakeMessage2D : AMessage<float, float, Vector2> { }