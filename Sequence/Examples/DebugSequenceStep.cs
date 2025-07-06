using Godot;

public partial class DebugSequenceStep : BaseSequenceStep
{
    [Export] string debugMessage;

    public override void _Process(double delta)
    {
        if (isPlaying)
        {
            Debug.Log($"{this.Name}: {debugMessage}");
            isComplete = true;
            isPlaying = false;
        }
    }

    public override void FinishStep()
    {
        Debug.Log($"{this.Name}: Finishing Step");
    }
    public override void LoadStep()
    {
        Debug.Log($"{this.Name}: Loading Step");
    }

    public override void StartStep()
    {
        Debug.Log($"{this.Name}: Starting Debug Step");
        isComplete = false;
        isPlaying = true;
    }

    public override void UnloadStep()
    {
        Debug.Log($"{this.Name}: Unloading Step");
    }
}
