struct InputDeviceEventDescription
{
    public string eventName;
    public InputDeviceEventType type;
    public int inputButtonIndex; //JoyButton.Start, Key.Escape
    public float inputButtonValue; //typicaly used for JoyAxis to denote the axis direction.
}
