using Godot;
using Godot.Collections;
using System;

public partial class InputButtonBinding : Resource
{
    [Export] public string ActionName = "Jump";
    [Export] public Key key = Key.Space;
    [Export] public JoyButton joyButton = JoyButton.A;
    [Export] public MouseButton mouseButton = MouseButton.Left;

    public void UpdateKey(Key newKey)
    {
        SwapKey(ActionName, key, newKey);
        key = newKey;
    }

    public void UpdateJoyButton(JoyButton newJoyButton)
    {
        SwapJoyButton(ActionName, joyButton, newJoyButton);
        joyButton = newJoyButton;
    }

    public void UpdateMouseButton(JoyButton newJoyButton)
    {
        SwapJoyButton(ActionName, joyButton, newJoyButton);
        joyButton = newJoyButton;
    }

    public void UpdateInputMap()
    {
        if (!InputMap.HasAction(ActionName))
        { 
            InputMap.AddAction(ActionName); 
        }

        InputMap.ActionAddEvent(ActionName, new InputEventKey()
        {
            Keycode = key
        });
        InputMap.ActionAddEvent(ActionName, new InputEventJoypadButton()
        {
            ButtonIndex = joyButton
        });
        InputMap.ActionAddEvent(ActionName, new InputEventMouseButton()
        {
            ButtonIndex = mouseButton
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
    private void SwapJoyButton(string action, JoyButton oldJoyButton, JoyButton newJoyButton)
    {
        var oldJoyButtonInput = new InputEventJoypadButton()
        {
            ButtonIndex = oldJoyButton
        };

        var newJoyButtonInput = new InputEventJoypadButton()
        {
            ButtonIndex = newJoyButton
        };

        if (!InputMap.HasAction(action))
        {
            InputMap.AddAction(action);
        }

        //we need to remove the old action
        if (InputMap.ActionHasEvent(action, oldJoyButtonInput))
        {
            InputMap.ActionEraseEvent(action, oldJoyButtonInput);
        }

        //now add the new key
        InputMap.ActionAddEvent(action, newJoyButtonInput);
    }
    private void SwapMouseButton(string action, MouseButton oldMouseButton, MouseButton newMouseButton)
    {
        var oldMouseButtonInput = new InputEventMouseButton()
        {
            ButtonIndex = oldMouseButton,
        };

        var newMouseButtonInput = new InputEventMouseButton()
        {
            ButtonIndex = newMouseButton
        };

        if (!InputMap.HasAction(action))
        {
            InputMap.AddAction(action);
        }

        //we need to remove the old action
        if (InputMap.ActionHasEvent(action, oldMouseButtonInput))
        {
            InputMap.ActionEraseEvent(action, oldMouseButtonInput);
        }

        //now add the new key
        InputMap.ActionAddEvent(action, newMouseButtonInput);
    }

    public bool GetActionPressed()
    {
        return Input.IsActionPressed(ActionName);
    }
    public bool GetActionJustPressed()
    {
        return Input.IsActionJustPressed(ActionName);
    }
    public bool GetActionJustReleased()
    {
        return Input.IsActionJustReleased(ActionName);
    }
}

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

public partial class InputAxisPressedBinding : Resource
{
    [Export] public string ActionName = "MoveUp";

    public JoyAxis Axis = JoyAxis.TriggerRight;

    public float AxisValue = 1.0f;
    public void SetAxisBinding(JoyAxis newAxis)
    {
        SwapJoyAxis(ActionName, Axis, newAxis);
        Axis = newAxis;
    }

    public void UpdateInputMap()
    {
        if (!InputMap.HasAction(ActionName)) { InputMap.AddAction(ActionName); }
        InputMap.ActionAddEvent(ActionName, new InputEventJoypadMotion()
        {
            Axis = Axis,
            AxisValue = 1.0f,
        });
    }
    private void SwapJoyAxis(string positiveAction, JoyAxis oldAxis, JoyAxis newAxis)
    {
        var positiveOld = new InputEventJoypadMotion()
        {
            Axis = oldAxis,
            AxisValue = 1.0f,
        };
        var positiveNew = new InputEventJoypadMotion()
        {
            Axis = newAxis,
            AxisValue = 1.0f,
        };
        if (!InputMap.HasAction(positiveAction))
        {
            InputMap.AddAction(positiveAction);
        }
        //we need to remove the old action
        if (InputMap.ActionHasEvent(positiveAction, positiveOld))
        {
            InputMap.ActionEraseEvent(positiveAction, positiveOld);
        }
        //now add the new key
        InputMap.ActionAddEvent(positiveAction, positiveNew);
    }
    public bool GetAxisPressed()
    {
        return Input.IsActionPressed(ActionName);
    }
}