using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class VirtualCamera : Node2D
{
    [Export] public int virtualCameraPriority;
    [Export] Vector2 windowSize = new Vector2(1920, 1080);
    //[Export] float orthographicSize;
    [Export] bool clampToIntegerScale;
    public Vector2 targetScreenSize;
    public Vector2 testScreenSize;

    //DEBUG ONLY
    [ExportCategory("Debug")]
    [Export] Vector2 testScreenResolution = new Vector2(1920, 1080);
    [ExportGroup("Settings")]
    [Export] private float debugBorderWidth = 30.0f;
    [Export] private Color debugVirtualBorderColor = Color.FromHtml($"FF8000");
    [Export] private Color debugBorderColor = Color.FromHtml($"0080FF");
    [Export] private Color debugTestBorderColor = Color.FromHtml($"8000FF");
    private ReferenceRect debugVirtualCameraBox;
    private ReferenceRect debugCameraBox;
    private ReferenceRect debugCameraTestBox;


    [ExportGroup("Read Only")]
    [Export] public Vector2 calculatedScale;
    [Export] public Vector2 calculatedScaleCurrent;
    [Export] public Vector2 calculatedScaleTest;
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        VirtualCameraBrain.RegisterCamera(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        VirtualCameraBrain.UnregisterCamera(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var screenResolution = GetViewportRect().Size;

        calculatedScale = ScaleScreen(screenResolution, this.windowSize);
        calculatedScaleCurrent = ScaleScreen(screenResolution, this.windowSize);

        targetScreenSize.X = (float)screenResolution.X / calculatedScale.X;
        targetScreenSize.Y = (float)screenResolution.Y / calculatedScale.Y;


        calculatedScaleTest = ScaleScreen(testScreenResolution, this.windowSize);
        testScreenSize.X = (float)testScreenResolution.X / calculatedScaleTest.X;
        testScreenSize.Y = (float)testScreenResolution.Y / calculatedScaleTest.Y;

        if (Engine.IsEditorHint())
        {
            // Code to execute when in editor.
            if (debugVirtualCameraBox == null)
            {
                debugVirtualCameraBox = new ReferenceRect();
                this.AddChild(debugVirtualCameraBox);
                debugVirtualCameraBox.Position = new Vector2();
                debugVirtualCameraBox.Size = this.windowSize;
            }
            debugVirtualCameraBox.BorderWidth = debugBorderWidth;
            debugVirtualCameraBox.BorderColor = debugVirtualBorderColor;
            debugVirtualCameraBox.Size = this.windowSize;
            debugVirtualCameraBox.Position = -(debugVirtualCameraBox.Size / 2.0f);
            debugVirtualCameraBox.ZIndex = 1000;


            if (debugCameraBox == null)
            {
                debugCameraBox = new ReferenceRect();
                this.AddChild(debugCameraBox);
                debugCameraBox.Position = new Vector2();
                debugCameraBox.Size = this.windowSize;
            }
            debugCameraBox.BorderWidth = debugBorderWidth;
            debugCameraBox.BorderColor = debugBorderColor;
            debugCameraBox.Size = targetScreenSize;
            debugCameraBox.Position = -(debugCameraBox.Size / 2.0f);
            debugCameraBox.ZIndex = 999;


            if (debugCameraTestBox == null)
            {
                debugCameraTestBox = new ReferenceRect();
                this.AddChild(debugCameraTestBox);
                debugCameraTestBox.Position = new Vector2();
                debugCameraTestBox.Size = this.windowSize;
            }
            debugCameraTestBox.BorderWidth = debugBorderWidth;
            debugCameraTestBox.BorderColor = debugTestBorderColor;
            debugCameraTestBox.Size = testScreenSize;
            debugCameraTestBox.Position = -(debugCameraTestBox.Size / 2.0f);
            debugCameraTestBox.ZIndex = 998;
        }
    }

    public Vector2 ScaleScreen(Vector2 inputResolution, Vector2 targetSize)
    {
        var windowSize = GetViewportRect().Size;
        var screenScale = new Vector2();
        //screenResolution.x(1920) / screenResolution.x(960) = calculatedScale.x(2)
        //screenResolution.x(1920) / screenResolution.x(3,840) = calculatedScale.x(0.5)
        screenScale.X = (float)inputResolution.X / targetSize.X;
        screenScale.Y = (float)inputResolution.Y / targetSize.Y;



        //either we're clamping the zoom to an integer scale 1x 2x etc
        if (clampToIntegerScale)
        {
            screenScale.X = Mathf.FloorToInt(screenScale.X);
            screenScale.X = Mathf.Clamp(screenScale.X, 1, 10_000);
            screenScale.Y = Mathf.FloorToInt(screenScale.Y);
            screenScale.Y = Mathf.Clamp(screenScale.Y, 1, 10_000);
        }
        //or we're just making sure the zoom ratio is never 0 for some reason.
        else
        {
            screenScale.X = Math.Clamp(screenScale.X, 0.0001f, 10_000.0f);
            screenScale.Y = Math.Clamp(screenScale.Y, 0.0001f, 10_000.0f);
        }

        if (screenScale.X > screenScale.Y)
        {
            screenScale.X = screenScale.Y;
        }
        else
        {
            screenScale.Y = screenScale.X;
        }
        return screenScale;
    }
}
