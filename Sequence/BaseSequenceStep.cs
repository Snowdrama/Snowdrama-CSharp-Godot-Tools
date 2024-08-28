using Godot;

[GlobalClass]
public abstract partial class BaseSequenceStep : Node, ISequenceStep
{
    protected bool isComplete = false;
    protected bool isPlaying = false;

    /// <summary>
    /// This forces the step complete and sets all the properties to the end of the step.
    /// 
    /// Used most commonly when the player inputs to skip the step by advancing the sequence
    /// </summary>
    public virtual void FinishStep() { }

    /// <summary>
    /// True if the step has finished playing.
    /// </summary>
    /// <returns>Is the step currently complete</returns>
    public virtual bool IsComplete() { return isComplete; }

    /// <summary>
    /// True if the step is currently playing once complete this will be false and IsComplete will be true.
    /// </summary>
    /// <returns>Is the step currently playing</returns>
    public virtual bool IsPlaying() { return isPlaying; }

    /// <summary>
    /// This is used to start the sequence step, this might for example play an animation
    /// </summary>
    public virtual void StartStep() { }



    /// <summary>
    /// This loads the node, spawning and connecting any things that are not loaded
    /// 
    /// This is called for all nodes in the sequence BEFORE the first step starts
    /// </summary>
    public virtual void LoadStep() { }

    /// <summary>
    /// this is meant to clean up any spawned items created in the Load Step
    /// 
    /// This is called for all nodes in the sequence AFTER the final step ends.
    /// </summary>
    public virtual void UnloadStep() { }
}
