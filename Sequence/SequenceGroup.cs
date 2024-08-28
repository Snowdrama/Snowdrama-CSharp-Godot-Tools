using Godot;
using Godot.Collections;

public partial class SequenceGroup : BaseSequenceStep
{
    [Export] Array<BaseSequenceStep> groupSteps = new Array<BaseSequenceStep>();
    public override void FinishStep()
    {
        for (int i = 0; groupSteps.Count > 0; i++)
        {
            groupSteps[i].FinishStep();
        }
    }

    public override bool IsComplete()
    {
        if (!base.isComplete)
        {
            base.isComplete = true;
            for (int i = 0; groupSteps.Count > 0; i++)
            {
                if (!groupSteps[i].IsComplete())
                {
                    base.isComplete = false;
                }
            }
        }

        return base.isComplete;
    }

    public override bool IsPlaying()
    {
        if (!base.isPlaying)
        {
            base.isPlaying = true;
            for (int i = 0; groupSteps.Count > 0; i++)
            {
                if (!groupSteps[i].IsPlaying())
                {
                    base.isPlaying = false;
                }
            }
        }

        return base.isPlaying;
    }

    public override void LoadStep()
    {
        for (int i = 0; groupSteps.Count > 0; i++)
        {
            groupSteps[i].LoadStep();
        }
    }

    public override void StartStep()
    {
        for (int i = 0; groupSteps.Count > 0; i++)
        {
            groupSteps[i].StartStep();
        }
    }

    public override void UnloadStep()
    {
        for (int i = 0; groupSteps.Count > 0; i++)
        {
            groupSteps[i].UnloadStep();
        }
    }
}
