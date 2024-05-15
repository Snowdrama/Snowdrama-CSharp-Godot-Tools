public interface ISequenceStep
{
    /// <summary>
    /// Loads the sequence, by setting all the actors to a specific state
    /// 
    /// This will do things like enable nodes or creeate nodes
    /// </summary>
    public void LoadStep();

    /// <summary>
    /// does any disposal if needed like if it created something temporary for this step
    /// </summary>
    public void DisposeStep();

    /// <summary>
    /// Sets all the actors to the final state of the sequence step
    /// </summary>
    public void ForceComplete();

    /// <summary>
    /// Play the sequence from the beginning
    /// 
    /// If autoAdvance is true, the sequence "automatically"
    /// </summary>
    public void PlaySequence();
}
