using Godot;

public partial class InputDevice_JoyAxisInput : Resource
{
    [Export] public string eventName;
    [Export] public InputDeviceEventType type;
    [Export] public JoyAxis inputButtonIndex; 
    [Export] public float inputButtonValue; 
}
