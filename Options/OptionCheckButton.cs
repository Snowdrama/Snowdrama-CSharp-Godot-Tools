using Godot;

public partial class OptionCheckButton : CheckButton
{
    [Export] string optionKey;

    bool localValue;
    public override void _Ready()
    {
        if (Options.HasBool(optionKey))
        {
            localValue = Options.GetBool(optionKey);
            Debug.Log($"[CheckButton: {this.Name}] Value From Config? {Options.GetBool(optionKey)}");
            this.ButtonPressed = localValue;
        }
        else
        {
            localValue = this.ButtonPressed;
            Options.SetBool(optionKey, localValue);
        }
    }

    public override void _Process(double delta)
    {
        //if something else toggled it
        if (Options.GetBool(optionKey) != localValue)
        {
            localValue = Options.GetBool(optionKey);
            this.ButtonPressed = localValue;
        }
    }
    public override void _EnterTree()
    {
        Debug.Log("Option Slider _EnterTree");
        base._EnterTree();
        this.Toggled += ToggleButton;
    }

    public override void _ExitTree()
    {
        Debug.Log("Option Slider _ExitTree");
        base._ExitTree();
        this.Toggled -= ToggleButton;
    }

    public void ToggleButton(bool newValue)
    {
        localValue = newValue;
        Options.SetBool(optionKey, newValue);
    }
}
