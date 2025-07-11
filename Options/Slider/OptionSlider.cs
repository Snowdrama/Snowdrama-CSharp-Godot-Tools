using Godot;

public partial class OptionSlider : HSlider
{
    [Export] string optionKey;

    double localValue;
    public override void _Ready()
    {
        this.VisibilityChanged += ValidateSlider;
        ValidateSlider();
    }

    private void ValidateSlider()
    {
        if (Options.HasDouble(optionKey))
        {
            localValue = Options.GetDouble(optionKey);
            Debug.Log($"[Slider: {this.Name}] Value From Config? {Options.GetDouble(optionKey)}");
            this.SetValueNoSignal(localValue);
        }
        else
        {
            localValue = this.Value;
            Options.SetDouble(optionKey, localValue);
        }
    }

    public override void _Process(double delta)
    {
        if (Options.GetDouble(optionKey) != localValue)
        {
            this.SetValueNoSignal(Options.GetDouble(optionKey));
        }
    }
    public override void _EnterTree()
    {
        Debug.Log("Option Slider _EnterTree");
        base._EnterTree();
        this.ValueChanged += SliderChanged;
    }

    public override void _ExitTree()
    {
        Debug.Log("Option Slider _ExitTree");
        base._ExitTree();
        this.ValueChanged -= SliderChanged;
    }

    public void SliderChanged(double newValue)
    {
        localValue = newValue;
        Options.SetDouble(optionKey, newValue);
    }
}
