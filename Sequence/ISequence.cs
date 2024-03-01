public interface ISequence
{
    /// <summary>
    /// play the sequence from the beginning
    /// or resumes the sequence if paused
    /// </summary>
    public void PlaySequence(bool autoAdvance);

    /// <summary>
    /// continues the sequence if paused
    /// </summary>
    public void AdvanceSequence();

    /// <summary>
    /// pauses the sequence
    /// note it doesn't stop the component only
    /// prevents the sequence from auto advancing
    /// </summary>
    public void PauseSequence();

    /// <summary>
    /// stops the sequence 
    /// </summary>
    public void StopSequence();
}