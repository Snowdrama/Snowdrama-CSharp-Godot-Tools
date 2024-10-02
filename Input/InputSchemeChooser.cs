using Godot;
using System;
public enum InputSchemeType
{
	KBM,
	Gamepad,
	Touch,
}
public class InputSchemeChangedMessage : AMessage<InputSchemeType> { }
public class InputSchemeChooser
{
    public static InputSchemeType SchemeType = InputSchemeType.KBM;

	public static void RequestSchemeType(InputSchemeType type)
	{
		if(SchemeType != type)
        {
            GD.Print($"Scheme type {SchemeType} changing to {type}");
            SchemeType = type;
            Messages.GetNoCount<InputSchemeChangedMessage>().Dispatch(type);
        }
    }
}
