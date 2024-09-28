using Godot;
using System;

public partial class UI_Button_SoundOnFoucs : AudioStreamPlayer
{

    BaseButton parentButton;
	public override void _Ready()
	{
		var parent = this.GetParent();

        if (parent is BaseButton pc)
        {
            
            parentButton = pc;
            parentButton.FocusEntered += Button_FocusEntered;
        }
	}

    public override void _ExitTree()
    {
        if(parentButton != null)
        {
            parentButton.FocusEntered -= Button_FocusEntered;
        }
    }

    private void Button_FocusEntered()
    {
        if (!parentButton.ButtonPressed)
        {
            this.Play();
        }
    }
}
