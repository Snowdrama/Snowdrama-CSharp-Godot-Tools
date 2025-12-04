using Godot;

public partial class UIButtonSound : Node
{
    BaseButton? parentButton;

    [Export] AudioStream? focusSound;
    [Export] AudioStream? pressedSound;
    [Export] AudioStream? hoverSound;

    AudioStreamPlayer focusPlayer;
    AudioStreamPlayer pressedPlayer;
    AudioStreamPlayer hoverPlayer;

    public UIButtonSound()
    {
        focusPlayer = new AudioStreamPlayer();
        pressedPlayer = new AudioStreamPlayer();
        hoverPlayer = new AudioStreamPlayer();
    }

    public override void _Ready()
    {
        var parent = this.GetParent();
        if (parent is not BaseButton)
        {
            Debug.LogError($"{parent.Name} is not a BaseButton, UIBUttonSound must be child of BaseButton type");
            return;
        }

        focusPlayer.Stream = focusSound;
        focusPlayer.VolumeDb = 0.0f;
        focusPlayer.PitchScale = 1.0f;
        focusPlayer.Autoplay = false;
        focusPlayer.StreamPaused = false;
        this.AddChild(focusPlayer);

        pressedPlayer.Stream = pressedSound;
        pressedPlayer.VolumeDb = 0.0f;
        pressedPlayer.PitchScale = 1.0f;
        pressedPlayer.Autoplay = false;
        pressedPlayer.StreamPaused = false;
        this.AddChild(pressedPlayer);

        hoverPlayer.Stream = hoverSound;
        hoverPlayer.VolumeDb = 0.0f;
        hoverPlayer.PitchScale = 1.0f;
        hoverPlayer.Autoplay = false;
        hoverPlayer.StreamPaused = false;
        this.AddChild(hoverPlayer);

        var parentButton = (BaseButton)parent;
        parentButton.FocusMode = Control.FocusModeEnum.All;
        parentButton.Pressed += ButtonPressed;
        parentButton.ButtonDown += ParentButton_ButtonDown;
        parentButton.ButtonUp += ParentButton_ButtonUp;
        parentButton.FocusEntered += Button_FocusEntered;
        parentButton.MouseEntered += ParentButton_MouseEntered;
        parentButton.MouseExited += ParentButton_MouseExited;
    }

    bool mouseOver = false;
    private void ParentButton_MouseExited()
    {
        mouseOver = false;
    }

    private void ParentButton_MouseEntered()
    {
        mouseOver = true;
        hoverPlayer.Play();
    }

    bool pressedDown = false;
    private void ParentButton_ButtonUp()
    {
        pressedDown = false;
    }


    private void ParentButton_ButtonDown()
    {
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
        pressedPlayer.Play();
    }
    private void Button_FocusEntered()
    {
        if (!pressedDown && !mouseOver)
        {
            focusPlayer.Play();
        }
    }
}
