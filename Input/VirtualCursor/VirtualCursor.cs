using Godot;
using System;

public partial class VirtualCursor : Node2D
{
    [Export] float cursorSpeed = 500.0f;
    [Export] float cursorAcceleration_Max = 1.0f;
    [Export] float cursorAcceleration = 0.0f;
    [Export] float cursorAccelerationSpeed = 0.0f;

    Vector2 inputDirection;
    Vector2 cursorPosition;


    Input.MouseModeEnum currentMouseMode = Input.MouseModeEnum.ConfinedHidden;
    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {


        if (inputDirection.LengthSquared() > 0.01f)
        {
            cursorAcceleration = cursorAcceleration.MoveTowards(cursorAcceleration_Max, (float)delta * cursorAccelerationSpeed);
            cursorPosition += inputDirection * cursorSpeed * cursorAcceleration * (float)delta;
            var newEvent = new InputEventMouseMotion();
            newEvent.Position = cursorPosition;
            newEvent.Relative = inputDirection;
            newEvent.Device = 69420;
            Input.ParseInputEvent(newEvent);
        }
        else
        {
            cursorAcceleration = cursorAcceleration.MoveTowards(0, (float)delta * cursorAccelerationSpeed * 2);
        }

        this.Position = cursorPosition;

        Input.MouseMode = currentMouseMode;
    }
    public override void _Input(InputEvent @event)
    {
        inputDirection = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown", 0.1f);

        if (@event is InputEventMouseMotion motion)
        {
            cursorPosition = motion.Position;
        }

        if(@event is InputEventMouseButton button)
        {
            if(button.Device == 0)
            {
                currentMouseMode = Input.MouseModeEnum.ConfinedHidden;
            }
        }

        if(@event is InputEventKey key)
        {
            if(key.Keycode == Key.Escape)
            {
                currentMouseMode = Input.MouseModeEnum.Visible;
            }
        }



        //Gamepad Xbox A, Switch B, PS Cross
        if (@event.IsActionPressed("ActionConfirm"))
        {
            //trigger the same thing as a mouse click
            var newEvent = new InputEventMouseButton();
            newEvent.ButtonIndex = MouseButton.Left;
            newEvent.Position = cursorPosition;
            newEvent.Pressed = true;
            newEvent.Device = 69420;
            Input.ParseInputEvent(newEvent);
        }
        else if(@event.IsActionReleased("ActionConfirm"))
        {
            //trigger the same thing as a mouse click
            var newEvent = new InputEventMouseButton();
            newEvent.ButtonIndex = MouseButton.Left;
            newEvent.Position = cursorPosition;
            newEvent.Pressed = false;
            newEvent.Device = 69420;
            Input.ParseInputEvent(newEvent);
        }
    }
}
