using Godot;
using System;

public partial class InputSchemeSwitcher : Node
{
	public override void _EnterTree()
	{
        this.ProcessMode = ProcessModeEnum.Always;
        this.SetProcessInput(true);
	}

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            InputSchemeChooser.RequestSchemeType(InputSchemeType.KBM);
        }
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventJoypadButton joyButton)
        {
            //don't consider dummy device 42089
            if (joyButton.Device != 42069)
            {
                InputSchemeChooser.RequestSchemeType(InputSchemeType.Gamepad);
            }
        }
        if (@event is InputEventJoypadMotion joyMotion)
        {
            //don't consider dummy device 42089
            if (joyMotion.Device != 42069)
            {
                InputSchemeChooser.RequestSchemeType(InputSchemeType.Gamepad);
            }
        }
    }
}
