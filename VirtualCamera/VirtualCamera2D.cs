using Godot;

[Tool, GlobalClass]
public partial class VirtualCamera2D : Node2D
{
    [Export] public bool forceActive = false;
    [Export] public bool isActive = false;
    [Export] private int activePriority = 10;
    [Export] private int inactivePriority = 0;
    [Export] public int virtualCameraPriority;

    [Export] private Vector2 WindowSize = new Vector2(1920, 1080);
    //[Export] float orthographicSize;
    [Export] private bool clampToIntegerScale;
    public Vector2 targetScreenSize;
    public Vector2 testScreenSize;

    [Export] public bool IgnoreRotation = true;

    [Export] public bool ScaleSmoothingEnabled;
    [Export] public bool PositionSmoothingEnabled;
    [Export] public float PositionSmoothingSpeed;

    [ExportGroup("Camera Bounds")]
    [Export] private bool useCameraBounds;
    [Export] private Rect2 cameraBounds;

    //DEBUG ONLY
    [ExportCategory("Debug")]
    [Export] private Vector2 TestScreenResolution = new Vector2(1920, 1080);
    [ExportGroup("Settings")]
    [Export] private float debugBorderWidth = 30.0f;
    [Export] private Color debugVirtualBorderColor = Color.FromHtml($"FF8000");
    [Export] private Color debugBorderColor = Color.FromHtml($"0080FF");
    [Export] private Color debugTestBorderColor = Color.FromHtml($"8000FF");
    private ReferenceRect debugVirtualCameraBox;
    private ReferenceRect debugCameraBox;
    private ReferenceRect debugCameraTestBox;


    [ExportGroup("Read Only")]
    [Export] private Vector2 currentPosition;
    [Export] private Vector2 halfExtents;
    [Export] public Vector2 cameraZoomLevel = new Vector2(1.0f, 1.0f);
    [Export] public Vector2 windowResScale;
    [Export] public Vector2 calculatedScaleCurrent;
    [Export] public Vector2 calculatedScaleTest;

    [Export] public Vector2 relativeZoom = new Vector2(1.0f, 1.0f);
    [Export] public Vector2 relativeZoomRange = new Vector2(0.1f, 2.0f);

    public override void _Ready()
    {
        base._Ready();
        currentPosition = this.GlobalPosition;
    }


    public void SetZoom(Vector2 targetZoom)
    {
        relativeZoom = targetZoom;
        relativeZoom = relativeZoom.Clamp(relativeZoomRange.X, relativeZoomRange.Y);
    }

    public void ChangeZoom(Vector2 changeAmount)
    {
        relativeZoom += changeAmount;
        relativeZoom = relativeZoom.Clamp(relativeZoomRange.X, relativeZoomRange.Y);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        VirtualCameraBrain2D.RegisterCamera(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        VirtualCameraBrain2D.UnregisterCamera(this);
    }

    // Called every frame. 'delta' is the elapsed lerpAmount since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);
        currentPosition = this.GlobalPosition;
        windowResScale = WindowSize.FindScaleFactor(GetViewportRect().Size);

        cameraZoomLevel = windowResScale / relativeZoom;

        if (Engine.IsEditorHint())
        {
            QueueRedraw();
            return;
        }

        if (isActive)
        {
            MarkAsActive();
        }
        else
        {
            MarkAsInactive();
        }

        if (useCameraBounds)
        {
            var cam = GetViewport().GetCamera2D();
            halfExtents = cam.GetViewportRect().Size / cam.Zoom * 0.5f;
            currentPosition.X = currentPosition.X.Clamp(
                cameraBounds.Position.X + halfExtents.X,
                cameraBounds.Size.X - halfExtents.X
            );
            currentPosition.Y = currentPosition.Y.Clamp(
                cameraBounds.Position.Y + halfExtents.Y,
                cameraBounds.Size.Y - halfExtents.Y
            );
            this.GlobalPosition = currentPosition;
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


    public void SetWindowSize(Vector2 setWindowSize)
    {
        WindowSize = setWindowSize;
        windowResScale = WindowSize.FindScaleFactor(GetViewportRect().Size);
    }


    public void SetCameraBounds(bool shouldUseCameraBounds, Rect2 setCameraBounds = new Rect2())
    {
        useCameraBounds = shouldUseCameraBounds;
        cameraBounds = setCameraBounds;
    }

    public override void _Draw()
    {
        base._Draw();

        if (Engine.IsEditorHint())
        {
            var halfWindowSize = WindowSize * 0.5f;
            var halfExtentsTest = testScreenSize * 0.5f;

            //Debug.Log($"halfExtentsTarget {halfWindowSize}");
            //Debug.Log($"halfExtentsTest {halfExtentsTest}");

            DrawLine(new Vector2(-halfExtentsTest.X, halfExtentsTest.Y), new Vector2(halfExtentsTest.X, halfExtentsTest.Y), Colors.Orange, debugBorderWidth);
            DrawLine(new Vector2(-halfExtentsTest.X, -halfExtentsTest.Y), new Vector2(halfExtentsTest.X, -halfExtentsTest.Y), Colors.Orange, debugBorderWidth);
            DrawLine(new Vector2(halfExtentsTest.X, -halfExtentsTest.Y), new Vector2(halfExtentsTest.X, halfExtentsTest.Y), Colors.Orange, debugBorderWidth);
            DrawLine(new Vector2(-halfExtentsTest.X, -halfExtentsTest.Y), new Vector2(-halfExtentsTest.X, halfExtentsTest.Y), Colors.Orange, debugBorderWidth);


            DrawLine(new Vector2(-halfWindowSize.X, halfWindowSize.Y), new Vector2(halfWindowSize.X, halfWindowSize.Y), Colors.Blue, debugBorderWidth);
            DrawLine(new Vector2(-halfWindowSize.X, -halfWindowSize.Y), new Vector2(halfWindowSize.X, -halfWindowSize.Y), Colors.Blue, debugBorderWidth);
            DrawLine(new Vector2(halfWindowSize.X, -halfWindowSize.Y), new Vector2(halfWindowSize.X, halfWindowSize.Y), Colors.Blue, debugBorderWidth);
            DrawLine(new Vector2(-halfWindowSize.X, -halfWindowSize.Y), new Vector2(-halfWindowSize.X, halfWindowSize.Y), Colors.Blue, debugBorderWidth);

            DrawLine(new Vector2(-halfWindowSize.X, halfWindowSize.Y) * relativeZoom, new Vector2(halfWindowSize.X, halfWindowSize.Y) * relativeZoom, Colors.Magenta, debugBorderWidth);
            DrawLine(new Vector2(-halfWindowSize.X, -halfWindowSize.Y) * relativeZoom, new Vector2(halfWindowSize.X, -halfWindowSize.Y) * relativeZoom, Colors.Magenta, debugBorderWidth);
            DrawLine(new Vector2(halfWindowSize.X, -halfWindowSize.Y) * relativeZoom, new Vector2(halfWindowSize.X, halfWindowSize.Y) * relativeZoom, Colors.Magenta, debugBorderWidth);
            DrawLine(new Vector2(-halfWindowSize.X, -halfWindowSize.Y) * relativeZoom, new Vector2(-halfWindowSize.X, halfWindowSize.Y) * relativeZoom, Colors.Magenta, debugBorderWidth);
        }
    }
}

