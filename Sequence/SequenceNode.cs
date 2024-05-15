using Godot;
using System;

public partial class SequenceNode : Node
{

    public SequenceState State { get; protected set; }

    protected Action onCompleted;

    /// <summary>
    /// sets the initial values of the sequence
    /// loads actors and images and such
    /// </summary>
    public virtual void LoadSequence()
    {
    }

    /// <summary>
    /// starts the playback of the node
    /// </summary>
    public virtual void PlaySequence(Action setOnCompleted)
    {
        this.State = SequenceState.Playing;
        onCompleted += setOnCompleted; 
    }

    /// <summary>
    /// sets the values to whatever they
    /// should be at the end of the sequence
    /// instantly
    /// </summary>
    public virtual void ForceComplete()
    {
        this.State = SequenceState.Completed;
        Completed();
    }

    /// <summary>
    /// completes the sequence naturally
    /// </summary>
    public virtual void Completed()
    {
        this.State = SequenceState.Completed;
        onCompleted?.Invoke();
    }

    /// <summary>
    /// Removes values and unloads 
    /// things like actors and stuff
    /// </summary>
    public virtual void UnloadSequence()
    {
    }
}
