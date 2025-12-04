using Godot;
using System;
public partial class OptionIntHSlider : HSlider
{
    [Export] string optionKey;

    [Export] int defaultValue = 0;
    int localValue = 0;

    public override void _Ready()
    {
        if (Options.HasInt(optionKey))
        {
            localValue = Options.GetInt(optionKey, defaultValue);
            this.SetValueNoSignal(localValue);
        }
        else
        {
            localValue = defaultValue;
            Options.SetInt(optionKey, localValue);
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        this.ValueChanged += SliderChanged;
        this.VisibilityChanged += OnVisibilityChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.ValueChanged -= SliderChanged;
        this.VisibilityChanged -= OnVisibilityChanged;
    }

    private void OnVisibilityChanged()
    {
        localValue = Options.GetInt(optionKey, defaultValue);
        this.SetValueNoSignal(localValue);
    }

    public void SliderChanged(double newValue)
    {
        Options.SetInt(optionKey, Mathf.FloorToInt(newValue));
    }
}
