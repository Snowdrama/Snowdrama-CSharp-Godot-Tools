using Godot;
using System;

public partial class UI_Button_Sounds : Node
{
    BaseButton parentButton;

    [Export] AudioStream focusSound;
    [Export] AudioStream pressedSound;

    AudioStreamPlayer focusPlayer;
    AudioStreamPlayer pressedPlayer;

    public override void _Ready()
    {
        focusPlayer = new AudioStreamPlayer();
        focusPlayer.Stream = focusSound;
        focusPlayer.VolumeDb = 0.0f;
        focusPlayer.PitchScale = 1.0f;
        focusPlayer.Autoplay = false;
        focusPlayer.StreamPaused = false;
        this.AddChild(focusPlayer);

        pressedPlayer = new AudioStreamPlayer();
        pressedPlayer.Stream = pressedSound;
        pressedPlayer.VolumeDb = 0.0f;
        pressedPlayer.PitchScale = 1.0f;
        pressedPlayer.Autoplay = false;
        pressedPlayer.StreamPaused = false;
        this.AddChild(pressedPlayer);

        var parent = this.GetParent();
        if (parent is BaseButton pc)
        {

            parentButton = pc;
            parentButton.FocusMode = Control.FocusModeEnum.All;
            parentButton.Pressed += ButtonPressed;
            parentButton.FocusEntered += Button_FocusEntered;
        }
    }

    public override void _ExitTree()
    {
        if (parentButton != null)
        {
            parentButton.Pressed -= ButtonPressed;
            parentButton.FocusEntered -= Button_FocusEntered;
        }
    }

    private void ButtonPressed()
    {
        if (parentButton != null && parentButton.ButtonPressed)
        {
            pressedPlayer.Play();
        }
    }
    private void Button_FocusEntered()
    {
        if (parentButton != null && !parentButton.ButtonPressed)
        {
            focusPlayer.Play();
        }
    }
}
