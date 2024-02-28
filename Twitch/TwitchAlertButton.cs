using Godot;
using System;

public partial class TwitchAlertButton : Button
{
	public AlertData alertData;
    public override void _Ready()
    {
        this.ButtonDown += TwitchAlertButton_ButtonDown;

        this.SizeFlagsHorizontal = SizeFlags.ExpandFill;
        this.SizeFlagsVertical = SizeFlags.ShrinkCenter;
    }
    private void TwitchAlertButton_ButtonDown()
    {
        Messages.Get<TwicthAlertMessage>().Dispatch(alertData);
    }
}
