using Godot;

public partial class AudioSlider : HSlider
{
    [Export] string optionKey;

    double currentValue;
    public override void _Ready()
    {
        this.MinValue = 0;
        this.MaxValue = 1.0;
        this.Step = 0.001;

        if (Options.HasDouble(optionKey))
        {
            currentValue = Mathf.Clamp(Options.GetDouble(optionKey, 0.8), 0.0, 1.0);
            this.SetValueNoSignal(Mathf.Lerp(this.MinValue, this.MaxValue, currentValue));
        }
        else
        {
            currentValue = (float)Mathf.InverseLerp((float)this.MinValue, (float)this.MaxValue, (float)this.Value);
            Options.SetDouble(optionKey, currentValue);
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
        currentValue = Mathf.Clamp(Options.GetDouble(optionKey, 0.5), 0.0, 1.0);
        this.SetValueNoSignal(Mathf.Lerp(this.MinValue, this.MaxValue, currentValue));
    }

    public void SliderChanged(double newValue)
    {
        var db = Mathf.LinearToDb(newValue);
        AudioManager.SetVolume(optionKey, (float)db);
        Options.SetDouble(optionKey, newValue);
    }
}
