using Godot;
using System;

public partial class AudioSlider : HSlider
{
    [Export] string optionKey;

    float normalizedLerpValue;
    public override void _Ready()
    {
        if (Options.HasFloat(optionKey))
        {
            normalizedLerpValue = Mathf.Clamp(Options.GetFloat(optionKey, 0.8f), 0.0f, 1.0f);
            this.SetValueNoSignal(Mathf.Lerp(this.MinValue, this.MaxValue, normalizedLerpValue));
        }
        else
        {
            normalizedLerpValue = (float)Mathf.InverseLerp((float)this.MinValue, (float)this.MaxValue, (float)this.Value);
            Options.SetFloat(optionKey, normalizedLerpValue);
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
        normalizedLerpValue = Mathf.Clamp(Options.GetFloat(optionKey, 0.5f), 0.0f, 1.0f);
        this.SetValueNoSignal(Mathf.Lerp(this.MinValue, this.MaxValue, normalizedLerpValue));
    }

    public void SliderChanged(double newValue)
    {
        normalizedLerpValue = (float)Mathf.InverseLerp((float)this.MinValue, (float)this.MaxValue, newValue);
        GD.PrintErr($"Value: {newValue} Normalized: {normalizedLerpValue}");
        AudioManager.SetVolume(optionKey, normalizedLerpValue);
        Options.SetFloat(optionKey, normalizedLerpValue);
    }
}
