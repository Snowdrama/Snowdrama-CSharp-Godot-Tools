using Godot;
using System;


[Tool]
public partial class TouchAnalogStick : Control
{
    public static Vector2 leftTouchAnalog;
    public static Vector2 rightTouchAnalog;


    [Export] Rect2 inputArea;
    [Export(PropertyHint.Range, "0,1")] Vector2 areaAnchorOffsetX = new Vector2(0, 1);
    [Export(PropertyHint.Range, "0,1")] Vector2 areaAnchorOffsetY = new Vector2(0, 1);
    [Export] bool useHalfScreenSize = true;
    [Export] bool useLeftHalf = true;


    [ExportCategory("Stick")]
    [Export] LayoutPreset joyOffsetAnchor;
    [Export] TextureRect onScreenImageJoy;
    [Export] Vector2 joySize = new Vector2(96, 96);
    Vector2 joyOffset
    {
        get
        {
            return -joySize * 0.5f;
        }
    }
    Vector2 joyHalfSize
    {
        get
        {
            return joySize * 0.5f;
        }
    }
    

    [ExportCategory("Stick BG")]
    [Export] LayoutPreset joyBGOffsetAnchor;
    [Export] TextureRect onScreenImageJoyBG;
    [Export] Vector2 joyBGSize = new Vector2(160, 160);

    Vector2 joyBGOffset
    {
        get
        {
            return -joyBGSize * 0.5f;
        }
    }

    Vector2 joyBGHalfSize
    {
        get
        {
            return joyBGSize * 0.5f;
        }
    }


    [ExportCategory("Released Position")]
    Vector2 analogDirection;
    [Export] float analogLength = 80;
    [Export] Vector2 releasedOffset = new Vector2(50, -50);
    [Export] Vector2 releasedAnchorPosition = new Vector2(0, 1);
    [ExportCategory("Released Position")]
    [Export] JoyAxis xAxis;
    [Export] JoyAxis YAxis;

    [ExportCategory("Fade When Not Used")]
    [Export] float timeUntilFade = 1.0f;
    [Export] float timeUntilFade_Max = 1.0f;
    [Export] float fadeAmount = 1.0f;
    [Export] float fadeAmount_Max = 1.0f;
    [Export] float imageFadeSpeed = 1.0f;
    [Export] Color joyColor = Colors.White;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed UpdateTimeMax since the previous frame.
    public override void _Process(double delta)
    {
        var screenSize = DisplayServer.WindowGetSize();
        if (useHalfScreenSize)
        {
            if (useLeftHalf)
            {
                inputArea.Position = Vector2.Zero + new Vector2(inputArea.Size.X * areaAnchorOffsetX.X, inputArea.Size.Y * areaAnchorOffsetY.X);
                inputArea.Size = new Vector2(screenSize.X / 2.0f, screenSize.Y) - new Vector2(inputArea.Size.X * (1.0f - areaAnchorOffsetX.Y), inputArea.Size.Y * (1.0f - areaAnchorOffsetY.Y));
            }
            else
            {
                inputArea.Position = Vector2.Zero + new Vector2(screenSize.X / 2.0f, 0.0f) + new Vector2(inputArea.Size.X * areaAnchorOffsetX.X, inputArea.Size.Y * areaAnchorOffsetY.X);
                inputArea.Size = new Vector2(screenSize.X / 2.0f, screenSize.Y) - new Vector2(inputArea.Size.X * (1.0f - areaAnchorOffsetX.Y), inputArea.Size.Y * (1.0f - areaAnchorOffsetY.Y));
            }
        }


        this.GlobalPosition = inputArea.Position;
        this.Size = inputArea.Size;
        
        if (!Engine.IsEditorHint())
        {
            if (joyMoving)
            {
                timeUntilFade = timeUntilFade_Max;
                fadeAmount = fadeAmount_Max;
            }
            else
            {
                if (timeUntilFade > 0.0f)
                {
                    timeUntilFade -= (float)delta * imageFadeSpeed;
                }
                else if (fadeAmount > 0.0f)
                {
                    fadeAmount -= (float)delta * imageFadeSpeed;
                }
            }
            joyColor.A = fadeAmount;
            onScreenImageJoy.Modulate = joyColor;
            onScreenImageJoyBG.Modulate = joyColor;
        }
        if (Engine.IsEditorHint())
        {
            QueueRedraw();
        }
    }

    bool joyMoving;
    Vector2 joyMovingStart;
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventScreenTouch touch)
        {
            if (touch.Pressed)
            {
                InputSchemeChooser.RequestSchemeType(InputSchemeType.Touch);
                if (inputArea.HasPoint(touch.Position))
                {
                    if (!joyMoving)
                    {
                        joyMoving = true;
                        joyMovingStart = touch.Position;
                    }

                    onScreenImageJoyBG.GlobalPosition = joyMovingStart - joyBGHalfSize;
                    analogDirection = (touch.Position - joyMovingStart);
                    onScreenImageJoy.GlobalPosition = (joyMovingStart - joyHalfSize) + analogDirection.LimitLength(analogLength);
                }
            }
            else
            {
                joyMoving = false;
                onScreenImageJoy.SetAnchorAndOffset(Side.Top, releasedAnchorPosition.Y, releasedOffset.Y - joyHalfSize.Y);
                onScreenImageJoy.SetAnchorAndOffset(Side.Bottom, releasedAnchorPosition.Y, releasedOffset.Y + joyHalfSize.Y);
                onScreenImageJoy.SetAnchorAndOffset(Side.Left, releasedAnchorPosition.X, releasedOffset.X - joyHalfSize.X);
                onScreenImageJoy.SetAnchorAndOffset(Side.Right, releasedAnchorPosition.X, releasedOffset.X + joyHalfSize.X);

                onScreenImageJoyBG.SetAnchorAndOffset(Side.Top, releasedAnchorPosition.Y, releasedOffset.Y - joyBGHalfSize.Y);
                onScreenImageJoyBG.SetAnchorAndOffset(Side.Bottom, releasedAnchorPosition.Y, releasedOffset.Y + joyBGHalfSize.Y);
                onScreenImageJoyBG.SetAnchorAndOffset(Side.Left, releasedAnchorPosition.X, releasedOffset.X - joyBGHalfSize.X);
                onScreenImageJoyBG.SetAnchorAndOffset(Side.Right, releasedAnchorPosition.X, releasedOffset.X + joyBGHalfSize.X);

                analogDirection = Vector2.Zero;
            }
        }

        if (@event is InputEventMouse mouse)
        {
            if(BitmaskExtensions.IsSet(mouse.ButtonMask, MouseButtonMask.Left))
            {
                InputSchemeChooser.RequestSchemeType(InputSchemeType.Touch);
                if (inputArea.HasPoint(mouse.Position))
                {
                    if (!joyMoving)
                    {
                        joyMoving = true;
                        joyMovingStart = mouse.Position;
                    }

                    onScreenImageJoyBG.GlobalPosition = joyMovingStart - joyBGHalfSize;
                    analogDirection = (mouse.Position - joyMovingStart);
                    onScreenImageJoy.GlobalPosition = (joyMovingStart - joyHalfSize) + analogDirection.LimitLength(analogLength);
                }
            }
            else
            {
                joyMoving = false;
                onScreenImageJoy.SetAnchorAndOffset(Side.Top, releasedAnchorPosition.Y, releasedOffset.Y - joyHalfSize.Y);
                onScreenImageJoy.SetAnchorAndOffset(Side.Bottom, releasedAnchorPosition.Y, releasedOffset.Y + joyHalfSize.Y);
                onScreenImageJoy.SetAnchorAndOffset(Side.Left, releasedAnchorPosition.X, releasedOffset.X - joyHalfSize.X);
                onScreenImageJoy.SetAnchorAndOffset(Side.Right, releasedAnchorPosition.X, releasedOffset.X + joyHalfSize.X);

                onScreenImageJoyBG.SetAnchorAndOffset(Side.Top, releasedAnchorPosition.Y, releasedOffset.Y - joyBGHalfSize.Y);
                onScreenImageJoyBG.SetAnchorAndOffset(Side.Bottom, releasedAnchorPosition.Y, releasedOffset.Y + joyBGHalfSize.Y);
                onScreenImageJoyBG.SetAnchorAndOffset(Side.Left, releasedAnchorPosition.X, releasedOffset.X - joyBGHalfSize.X);
                onScreenImageJoyBG.SetAnchorAndOffset(Side.Right, releasedAnchorPosition.X, releasedOffset.X + joyBGHalfSize.X);

                analogDirection = Vector2.Zero;
            }

            if (xAxis == JoyAxis.LeftX)
            {
                leftTouchAnalog.X = analogDirection.X * (1.0f / analogLength);
            }
            if (YAxis == JoyAxis.LeftY)
            {
                leftTouchAnalog.Y = analogDirection.Y * (1.0f / analogLength);
            }
            if (xAxis == JoyAxis.RightX)
            {
                rightTouchAnalog.X = analogDirection.X * (1.0f / analogLength);
            }
            if (YAxis == JoyAxis.RightY)
            {
                rightTouchAnalog.Y = analogDirection.Y * (1.0f / analogLength);
            }

            InputEventJoypadMotion leftAnalogX = new InputEventJoypadMotion();
            leftAnalogX.Axis = xAxis;
            leftAnalogX.AxisValue = analogDirection.X * (1.0f / analogLength);
            leftAnalogX.Device = 42069;

            InputEventJoypadMotion leftAnalogY = new InputEventJoypadMotion();
            leftAnalogY.Axis = YAxis;
            leftAnalogY.AxisValue = analogDirection.Y * (1.0f / analogLength);
            leftAnalogY.Device = 42069;
            Input.ParseInputEvent(leftAnalogX);
            Input.ParseInputEvent(leftAnalogY);
        }
    }

    
    public override void _Draw()
    {
        base._Draw();
        //DrawLine(Vector2.Zero, new Vector2(0, inputArea.Size.Y), Colors.Cyan);
        //DrawLine(Vector2.Zero, new Vector2(inputArea.Size.X, 0), Colors.Cyan);
        //DrawLine(Vector2.Zero+ new Vector2(0, inputArea.Size.Y), new Vector2(inputArea.Size.X, inputArea.Size.Y), Colors.Cyan);
        //DrawLine(Vector2.Zero+ new Vector2(inputArea.Size.X, 0), new Vector2(inputArea.Size.X, inputArea.Size.Y), Colors.Cyan);
    }
}
