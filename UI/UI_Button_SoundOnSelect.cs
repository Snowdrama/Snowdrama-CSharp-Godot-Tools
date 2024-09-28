using Godot;
using System;

public partial class UI_Button_SoundOnSelect : AudioStreamPlayer
{
    BaseButton parentButton;
    public override void _Ready()
    {
        //get the parent
        var parent = this.GetParent();

        if (parent is BaseButton pc)
        {
            parentButton = pc;
            parentButton.Pressed += ButtonPressed;
        }
    }

    public override void _ExitTree()
    {
        if (parentButton != null)
        {
            parentButton.Pressed -= ButtonPressed;
        }
    }

    private void ButtonPressed()
    {
        this.Play();
    }
}
