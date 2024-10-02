using Godot;
using System;

public partial class OptionFloatHSlider : HSlider
{
    [Export] string optionKey;

    [Export] float defaultValue = 0.0f;
    float localValue = 0.0f;

    public override void _Ready()
    {
        if (Options.HasFloat(optionKey))
        {
            localValue = Options.GetFloat(optionKey, defaultValue);
            this.SetValueNoSignal(localValue);
        }
        else
        {
            localValue = defaultValue;
            Options.SetFloat(optionKey, localValue);
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
        localValue = Options.GetFloat(optionKey, defaultValue);
        this.SetValueNoSignal(localValue);
    }

    public void SliderChanged(double newValue)
    {
        Options.SetFloat(optionKey, (float)newValue);
    }
}
