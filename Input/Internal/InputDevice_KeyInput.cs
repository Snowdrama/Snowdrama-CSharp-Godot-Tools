using Godot;

public partial class InputDevice_KeyInput : Resource
{
    [Export] public string eventName;
    [Export] public InputDeviceEventType type;
    [Export] public Key inputButtonIndex;
}
