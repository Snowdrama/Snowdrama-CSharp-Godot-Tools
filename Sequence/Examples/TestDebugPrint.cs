using Godot;
using System;

public partial class TestDebugPrint : SequenceNode
{
    [Export] string DebugPrintText;
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public override void PlaySequence(Action setOnCompleted)
    {
        base.PlaySequence(setOnCompleted);
        GD.Print(DebugPrintText);
        this.State = SequenceState.Completed;
    }
}
