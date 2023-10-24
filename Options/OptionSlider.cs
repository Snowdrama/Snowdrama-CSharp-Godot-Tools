using Godot;
using System;

public partial class OptionSlider : HSlider
{
    [Export] Options optionsResource;
    [Export] string optionKey;
    public override void _Ready()
	{
        GD.Print("Option Slider _Ready");
        GD.Print($"Value from config? {optionsResource.GetDouble(optionKey)}");
        this.SetValueNoSignal(optionsResource.GetDouble(optionKey));
	}
    public override void _EnterTree()
    {
        GD.Print("Option Slider _EnterTree");
        base._EnterTree();
        this.ValueChanged += SliderChanged;
    }

    public override void _ExitTree()
    {
        GD.Print("Option Slider _ExitTree");
        base._ExitTree();
        this.ValueChanged -= SliderChanged;
    }

    public void SliderChanged(double newValue)
    {
        optionsResource.SetDouble(optionKey, newValue);
    }
}
