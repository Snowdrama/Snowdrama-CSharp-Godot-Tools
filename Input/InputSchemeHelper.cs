using Godot;
public enum InputSchemeType
{
    None,
    KBM,
    Gamepad,
    Touch,
}
public class InputSchemeChangedMessage : AMessage<InputSchemeType> { }
public class InputSchemeHelper
{
    public static InputSchemeType SchemeType = InputSchemeType.None;

    [Signal]
    public delegate void InputSchemeChangedEventHandler(InputSchemeType newScheme);
    public static InputSchemeChangedEventHandler? InputSchemeChanged;

    public static void RequestSchemeType(InputSchemeType type)
    {
        if (SchemeType != type)
        {
            Debug.Log($"Scheme type {SchemeType} changing to {type}");
            SchemeType = type;
            Messages.GetNoCount<InputSchemeChangedMessage>().Dispatch(type);
            InputSchemeChanged?.Invoke(type);
        }
    }
}
