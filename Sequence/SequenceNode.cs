using Godot;
using System;

public partial class SequenceNode : Node
{

    Sequence _parentSequence;

    public SequenceState State { get; protected set; }

	public override void _Ready()
	{
        switch (State)
        {
            case SequenceState.Stopped:
                break;
            case SequenceState.Playing:
                break;
            case SequenceState.Paused:
                break;
            case SequenceState.Completed:
                break;
        }
    }

    public void RegisterParentSequence(Sequence parentSequence)
    {
        _parentSequence = parentSequence;
    }

    public virtual void LoadSequence()
    {
    }
    public virtual void PlaySequence()
    {
    }
    public virtual void ForceComplete()
    {
    }
    public virtual void UnloadSequence()
    {
    }
}
