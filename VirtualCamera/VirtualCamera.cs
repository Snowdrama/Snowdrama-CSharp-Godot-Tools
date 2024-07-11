using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[Tool]
public partial class VirtualCamera : Node2D
{
    [Export] int activePriority = 10;
    [Export] int inactivePriority = 0;
    [Export] public int virtualCameraPriority;
    
    Vector2 _windowSize = new Vector2(1920, 1080);
    [Export]
    Vector2 WindowSize
    {
        get { return _windowSize; }
        set
        {
            _windowSize = value;
            QueueRedraw();
        }
    }
    //[Export] float orthographicSize;
    [Export] bool clampToIntegerScale;
    public Vector2 targetScreenSize;
    public Vector2 testScreenSize;

    //DEBUG ONLY
    [ExportCategory("Debug")]
    Vector2 _testScreenResolution = new Vector2(1920, 1080);
    [Export]
    Vector2 TestScreenResolution
    {
        get { return _testScreenResolution; }
        set
        {
            _testScreenResolution = value;
            QueueRedraw();
        }
    }
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

    // Called every frame. 'delta' is the elapsed lerpAmount since the previous frame.
    public override void _Process(double delta)
    {


        if (!Engine.IsEditorHint())
        {
            UpdateScreenSize();
        }
    }

    public void MarkAsActive()
    {
        virtualCameraPriority = activePriority;

    }
    public void MarkAsInactive()
    {
        virtualCameraPriority = inactivePriority;
    }
    public void UpdateScreenSize()
    {
        var screenResolution = GetViewportRect().Size;

        calculatedScale = ScaleScreen(screenResolution, this._windowSize);
        calculatedScaleCurrent = ScaleScreen(screenResolution, this._windowSize);

        targetScreenSize.X = (float)screenResolution.X / calculatedScale.X;
        targetScreenSize.Y = (float)screenResolution.Y / calculatedScale.Y;


        calculatedScaleTest = ScaleScreen(_testScreenResolution, this._windowSize);

        testScreenSize.X = (float)_testScreenResolution.X / calculatedScaleTest.X;
        testScreenSize.Y = (float)_testScreenResolution.Y / calculatedScaleTest.Y;
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

    public override void _Draw()
    {
        base._Draw();

        if (Engine.IsEditorHint())
        {
            UpdateScreenSize();
            var halfWindowSize = _windowSize * 0.5f;
            var halfExtentsTest = testScreenSize * 0.5f;

            //GD.Print($"halfExtentsTarget {halfWindowSize}");
            //GD.Print($"halfExtentsTest {halfExtentsTest}");

            DrawLine(new Vector2(-halfExtentsTest.X, halfExtentsTest.Y), new Vector2(halfExtentsTest.X, halfExtentsTest.Y), Colors.Orange, 3.0f);
            DrawLine(new Vector2(-halfExtentsTest.X, -halfExtentsTest.Y), new Vector2(halfExtentsTest.X, -halfExtentsTest.Y), Colors.Orange, 3.0f);
            DrawLine(new Vector2(halfExtentsTest.X, -halfExtentsTest.Y), new Vector2(halfExtentsTest.X, halfExtentsTest.Y), Colors.Orange, 3.0f);
            DrawLine(new Vector2(-halfExtentsTest.X, -halfExtentsTest.Y), new Vector2(-halfExtentsTest.X, halfExtentsTest.Y), Colors.Orange, 3.0f);


            DrawLine(new Vector2(-halfWindowSize.X, halfWindowSize.Y), new Vector2(halfWindowSize.X, halfWindowSize.Y), Colors.Blue, 3.0f);
            DrawLine(new Vector2(-halfWindowSize.X, -halfWindowSize.Y), new Vector2(halfWindowSize.X, -halfWindowSize.Y), Colors.Blue, 3.0f);
            DrawLine(new Vector2(halfWindowSize.X, -halfWindowSize.Y), new Vector2(halfWindowSize.X, halfWindowSize.Y), Colors.Blue, 3.0f);
            DrawLine(new Vector2(-halfWindowSize.X, -halfWindowSize.Y), new Vector2(-halfWindowSize.X, halfWindowSize.Y), Colors.Blue, 3.0f);
        }
    }
}

