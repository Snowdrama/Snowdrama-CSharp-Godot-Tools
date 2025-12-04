using Godot;

public partial class InputDevice_JoyButtonInput : Resource
{
    [Export] public string eventName;
    [Export] public InputDeviceEventType type;
    [Export] public JoyButton inputButtonIndex;
}
