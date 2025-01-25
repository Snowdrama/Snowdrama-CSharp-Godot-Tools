using Godot;

public partial class InputDevice_Configuration : Resource
{
    [Export] Godot.Collections.Array<InputDevice_KeyInput> _defaultKeys;
    [Export] Godot.Collections.Array<InputDevice_JoyButtonInput> _defaultJoyButtons;
    [Export] Godot.Collections.Array<InputDevice_JoyAxisInput> _defaultJoyAxis;

    public Godot.Collections.Array<InputDevice_KeyInput> DefaultKeys{
        get { return _defaultKeys; }    
    }
    public Godot.Collections.Array<InputDevice_JoyButtonInput> DefaultJoyButtons{
        get { return _defaultJoyButtons; }    
    }
    public Godot.Collections.Array<InputDevice_JoyAxisInput> DefaultJoyAxis{
        get { return _defaultJoyAxis; }    
    }
}
