using Godot;

public partial class InputDirectionBinding : Resource
{
    [Export] public string UpAction = "MoveUp";
    [Export] public string DownAction = "MoveDown";
    [Export] public string LeftAction = "MoveLeft";
    [Export] public string RightAction = "MoveRight";

    [Export] public Key UpKey = Key.Up;
    [Export] public Key DownKey = Key.Down;
    [Export] public Key LeftKey = Key.Left;
    [Export] public Key RightKey = Key.Right;

    [Export] public JoyAxis horizontalAxis = JoyAxis.LeftX;
    [Export] public JoyAxis verticalAxis = JoyAxis.LeftY;

    public void SetDirectionBinding_UpKey(Key newKey)
    {
        SwapKey(UpAction, UpKey, newKey);
        UpKey = newKey;
    }
    public void SetDirectionBinding_DownKey(Key newKey)
    {
        SwapKey(DownAction, DownKey, newKey);
        DownKey = newKey;
    }

    public void SetDirectionBinding_LeftKey(Key newKey)
    {
        SwapKey(LeftAction, LeftKey, newKey);
        LeftKey = newKey;
    }
    public void SetDirectionBinding_RightKey(Key newKey)
    {
        SwapKey(RightAction, RightKey, newKey);
        RightKey = newKey;
    }
    public void SetDirectionBinding_VerticalAxis(JoyAxis newAxis)
    {
        SwapJoyAxis(DownAction, UpAction, verticalAxis, newAxis);
        verticalAxis = newAxis;
    }
    public void SetDirectionBinding_HorizontalAxis(JoyAxis newAxis)
    {
        SwapJoyAxis(RightAction, LeftAction, horizontalAxis, newAxis);
        horizontalAxis = newAxis;
    }

    public void UpdateInputMap()
    {
        if (!InputMap.HasAction(UpAction)) { InputMap.AddAction(UpAction); }
        if (!InputMap.HasAction(DownAction)) { InputMap.AddAction(DownAction); }
        if (!InputMap.HasAction(LeftAction)) { InputMap.AddAction(LeftAction); }
        if (!InputMap.HasAction(RightAction)) { InputMap.AddAction(RightAction); }
        InputMap.ActionAddEvent(UpAction, new InputEventJoypadMotion()
        {
            Axis = verticalAxis,
            AxisValue = -1.0f,
        });

        InputMap.ActionAddEvent(DownAction, new InputEventJoypadMotion()
        {
            Axis = verticalAxis,
            AxisValue = 1.0f,
        });

        InputMap.ActionAddEvent(LeftAction, new InputEventJoypadMotion()
        {
            Axis = horizontalAxis,
            AxisValue = -1.0f,
        });

        InputMap.ActionAddEvent(RightAction, new InputEventJoypadMotion()
        {
            Axis = horizontalAxis,
            AxisValue = 1.0f,
        });

        InputMap.ActionAddEvent(UpAction, new InputEventKey()
        {
            Keycode = UpKey,
        });

        InputMap.ActionAddEvent(DownAction, new InputEventKey()
        {
            Keycode = DownKey,
        });

        InputMap.ActionAddEvent(LeftAction, new InputEventKey()
        {
            Keycode = LeftKey,
        });

        InputMap.ActionAddEvent(RightAction, new InputEventKey()
        {
            Keycode = RightKey,
        });
    }

    private void SwapKey(string action, Key oldKey, Key newKey)
    {
        var oldKeyInput = new InputEventKey()
        {
            Keycode = oldKey
        };

        var newKeyInput = new InputEventKey()
        {
            Keycode = newKey
        };

        if (!InputMap.HasAction(action))
        {
            InputMap.AddAction(action);
        }

        //we need to remove the old action
        if (InputMap.ActionHasEvent(action, oldKeyInput))
        {
            InputMap.ActionEraseEvent(action, oldKeyInput);
        }

        //now add the new key
        InputMap.ActionAddEvent(action, newKeyInput);
    }

    private void SwapJoyAxis(string positiveAction, string negativeAction, JoyAxis oldAxis, JoyAxis newAxis)
    {
        var positiveOld = new InputEventJoypadMotion()
        {
            Axis = oldAxis,
            AxisValue = 1.0f,
        };

        var negativeOld = new InputEventJoypadMotion()
        {
            Axis = oldAxis,
            AxisValue = 1.0f,
        };

        var positiveNew = new InputEventJoypadMotion()
        {
            Axis = newAxis,
            AxisValue = 1.0f,
        };

        var negativeNew = new InputEventJoypadMotion()
        {
            Axis = newAxis,
            AxisValue = 1.0f,
        };

        if (!InputMap.HasAction(positiveAction))
        {
            InputMap.AddAction(positiveAction);
        }
        if (!InputMap.HasAction(negativeAction))
        {
            InputMap.AddAction(negativeAction);
        }

        //we need to remove the old action
        if (InputMap.ActionHasEvent(positiveAction, positiveOld))
        {
            InputMap.ActionEraseEvent(positiveAction, positiveOld);
        }
        if (InputMap.ActionHasEvent(negativeAction, negativeOld))
        {
            InputMap.ActionEraseEvent(negativeAction, negativeOld);
        }

        //now add the new key
        InputMap.ActionAddEvent(positiveAction, positiveNew);
        InputMap.ActionAddEvent(negativeAction, negativeNew);
    }

    public Vector2 GetAxis()
    {
        return Input.GetVector(LeftAction, RightAction, UpAction, DownAction);
    }
    public float GetHorizontalAxis()
    {
        return Input.GetAxis(LeftAction, RightAction);
    }
    public float GetVerticalAxis()
    {
        return Input.GetAxis(UpAction, DownAction);
    }
}
