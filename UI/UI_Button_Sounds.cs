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
            parentButton.ButtonDown += ParentButton_ButtonDown;
            parentButton.ButtonUp += ParentButton_ButtonUp;
            parentButton.FocusEntered += Button_FocusEntered;
            parentButton.MouseEntered += ParentButton_MouseEntered;
            parentButton.MouseExited += ParentButton_MouseExited;
        }
    }

    bool mouseOver = false;
    private void ParentButton_MouseExited()
    {
        mouseOver = false;
    }

    private void ParentButton_MouseEntered()
    {
        mouseOver = true;
    }

    bool pressedDown = false;
    private void ParentButton_ButtonUp()
    {
        Debug.Log($"Button {parentButton.Name} ButtonUp");
        pressedDown = false;
    }


    private void ParentButton_ButtonDown()
    {
        Debug.Log($"Button {parentButton.Name} ButtonDown");
        pressedDown = true;
    }

    public override void _ExitTree()
    {
        if (parentButton != null)
        {
            parentButton.Pressed -= ButtonPressed;
            parentButton.ButtonDown -= ParentButton_ButtonDown;
            parentButton.ButtonUp -= ParentButton_ButtonUp;
            parentButton.FocusEntered -= Button_FocusEntered;
        }
    }

    private void ButtonPressed()
    {
        Debug.Log($"Button {parentButton.Name} ButtonPressed");
        pressedPlayer.Play();
    }
    private void Button_FocusEntered()
    {
        Debug.Log($"Button {parentButton.Name} FocusEntered");
        if (!pressedDown && !mouseOver)
        {
            focusPlayer.Play();
        }
    }
}
