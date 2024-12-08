using Godot;
using System;
public enum InputSchemeType
{
    None,
	KBM,
	Gamepad,
	Touch,
}
public class InputSchemeChangedMessage : AMessage<InputSchemeType> { }
public class InputSchemeChooser
{
    public static InputSchemeType SchemeType = InputSchemeType.None;

    static Action<InputSchemeType> onSchemeChangedEvent;
	
    public void AddSchemeChangedListener(Action<InputSchemeType> onSchemeChanged)
    {
        onSchemeChangedEvent += onSchemeChanged;
    }

    public void RemoveSchemeChangedListener(Action<InputSchemeType> onSchemeChanged)
    {
        onSchemeChangedEvent -= onSchemeChanged;
    }
    
    public static void RequestSchemeType(InputSchemeType type)
	{
		if(SchemeType != type)
        {
            GD.Print($"Scheme type {SchemeType} changing to {type}");
            SchemeType = type;
            Messages.GetNoCount<InputSchemeChangedMessage>().Dispatch(type);
            onSchemeChangedEvent?.Invoke(type);
        }
    }
}
