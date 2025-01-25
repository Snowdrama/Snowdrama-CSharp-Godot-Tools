using Godot;
using Godot.Collections;
using System;

public partial class InputButtonBinding : Resource
{
    [Export] public string ActionName = "Jump";
    [Export] public Key key = Key.None;
    [Export] public Key modifier = Key.None;
    [Export] public JoyButton joyButton = JoyButton.Invalid;
    [Export] public MouseButton mouseButton = MouseButton.None;

    [Export] public bool CtrlModifier = false;
    [Export] public bool AltModifier = false;
    [Export] public bool ShiftModifier = false;
    [Export] public bool MetaModifier = false;

    public void UpdateKey(
        Key newKey,
        bool ctrlPressed = false,
        bool shiftPressed = false,
        bool altPressed = false,
        bool metaPressed = false
    )
    {
        SwapKey(ActionName, key, newKey, ctrlPressed, shiftPressed, altPressed, metaPressed);
        key = newKey;
        CtrlModifier = ctrlPressed;
        ShiftModifier = shiftPressed;
        AltModifier = altPressed;
        MetaModifier = metaPressed;
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
            Keycode = key,
            CtrlPressed = CtrlModifier,
            ShiftPressed = ShiftModifier,
            AltPressed = AltModifier,
            MetaPressed = MetaModifier,
        });
        InputMap.ActionAddEvent(ActionName, new InputEventJoypadButton()
        {
            ButtonIndex = joyButton,
            
        });
        InputMap.ActionAddEvent(ActionName, new InputEventMouseButton()
        {
            ButtonIndex = mouseButton
        });
    }

    private void SwapKey(
        string action, 
        Key oldKey, 
        Key newKey, 
        bool ctrlPressed = false, 
        bool shiftPressed = false, 
        bool altPressed = false,
        bool metaPressed = false
    )
    {
        var oldKeyInput = new InputEventKey()
        {
            Keycode = oldKey,
            CtrlPressed = CtrlModifier,
            ShiftPressed = ShiftModifier,
            AltPressed = AltModifier,
            MetaPressed = MetaModifier,
        };

        key = newKey;
        CtrlModifier = ctrlPressed;
        ShiftModifier = shiftPressed;
        AltModifier = altPressed;
        MetaModifier = metaPressed;

        var newKeyInput = new InputEventKey()
        {
            Keycode = newKey,
            CtrlPressed = ctrlPressed,
            ShiftPressed = shiftPressed,
            AltPressed = altPressed,
            MetaPressed = metaPressed,
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
