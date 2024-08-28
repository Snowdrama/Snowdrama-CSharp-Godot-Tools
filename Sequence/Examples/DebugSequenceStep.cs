using Godot;
using System;

public partial class DebugSequenceStep : BaseSequenceStep
{
    [Export] string debugMessage;
    
    public override void _Process(double delta)
	{
        if (isPlaying)
        {
            GD.Print($"{this.Name}: {debugMessage}");
            isComplete = true;
            isPlaying = false;
        }
	}

    public override void FinishStep()
    {
        GD.Print($"{this.Name}: Finishing Step");
    }
    public override void LoadStep()
    {
        GD.Print($"{this.Name}: Loading Step");
    }

    public override void StartStep()
    {
        GD.Print($"{this.Name}: Starting Debug Step");
        isComplete = false;
        isPlaying = true;
    }

    public override void UnloadStep()
    {
        GD.Print($"{this.Name}: Unloading Step");
    }
}
