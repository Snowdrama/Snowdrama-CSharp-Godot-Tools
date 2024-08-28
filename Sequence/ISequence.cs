public interface ISequenceStep
{
    public void LoadStep();
    public void StartStep();
    public bool IsPlaying();
    public bool IsComplete();
    public void FinishStep();
    public void UnloadStep();
}
