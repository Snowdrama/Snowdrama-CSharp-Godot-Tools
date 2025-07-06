using Godot;
using Godot.Collections;

public partial class Sequence : Node
{
    [Export(PropertyHint.NodeType, "BaseSequenceStep")] Array<BaseSequenceStep> sequenceStepList = new Array<BaseSequenceStep>();
    bool isPlaying;
    int currentStepIndex = -1;

    [Export] bool playOnStart = false;
    [Export] bool autoPlay = false;
    [Export] float autoPlayWaitTimeMax = 1.0f;
    float autoPlayWaitTime = 1.0f;

    public override void _EnterTree()
    {
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            var child = this.GetChild(i);

            if (child is BaseSequenceStep step)
            {
                if (!sequenceStepList.Contains(step))
                {
                    sequenceStepList.Add(step);
                }
            }
        }
        if (!InputMap.HasAction("NextSequenceStep"))
        {
            InputMap.AddAction("NextSequenceStep");
            InputMap.ActionAddEvent("NextSequenceStep", new InputEventKey()
            {
                Keycode = Key.F5,
                PhysicalKeycode = Key.F5,
                KeyLabel = Key.F5,
            });
        }
        if (playOnStart)
        {
            StartSequence();
        }
    }
    public void StartSequence()
    {
        if (sequenceStepList.Count <= 0)
        {
            Debug.LogError($"Hey you forgot to put any sequence steps in {this.Name}");
            return;
        }

        Debug.Log($"Loading the sequence steps for {this.Name}");
        foreach (var item in sequenceStepList)
        {
            item.LoadStep();
        }

        isPlaying = true;
        currentStepIndex = 0;
        currentStep = sequenceStepList[currentStepIndex];
        currentStepState = SequenceStepState.StartStep;
    }

    public void FinishSequence()
    {
        Debug.Log($"Unloading the sequence steps for {this.Name}");
        foreach (var item in sequenceStepList)
        {
            item.UnloadStep();
        }
    }

    public BaseSequenceStep currentStep;
    public SequenceStepState currentStepState;
    public override void _Process(double delta)
    {
        if (!isPlaying)
        {
            //Debug.Log($"{this.Name}: Not Playing");
            return;
        }

        if (currentStepIndex < 0)
        {
            //Debug.Log($"{this.Name}: CurrentStep not valid: {currentStepIndex}");
            return;
        }

        if (currentStepIndex >= sequenceStepList.Count)
        {
            //Debug.Log($"{this.Name}: CurrentStep not valid: {currentStepIndex}");
            return;
        }

        if (currentStep == null)
        {
            //Debug.Log($"{this.Name}: CurrentStep not set");
            return;
        }

        switch (currentStepState)
        {
            case SequenceStepState.None:
                break;
            case SequenceStepState.StartStep:
                currentStep.StartStep();
                currentStepState = SequenceStepState.PlayingStep;
                autoPlayWaitTime = autoPlayWaitTimeMax;
                break;
            case SequenceStepState.PlayingStep:
                if (currentStep.IsComplete())
                {
                    currentStepState = SequenceStepState.FinishStep;
                }


                if (Input.IsActionJustPressed("NextSequenceStep"))
                {
                    currentStepState = SequenceStepState.FinishStep;
                }
                break;
            case SequenceStepState.FinishStep:
                currentStep.FinishStep();
                currentStepState = SequenceStepState.WaitingForInput;
                break;
            case SequenceStepState.WaitingForInput:
                if (autoPlay)
                {
                    autoPlayWaitTime -= (float)delta;
                    if (autoPlayWaitTime <= 0)
                    {
                        autoPlayWaitTime = autoPlayWaitTimeMax;
                        currentStepState = SequenceStepState.TryLoadNextStep;
                    }

                    if (Input.IsActionJustPressed("NextSequenceStep"))
                    {
                        currentStepState = SequenceStepState.TryLoadNextStep;
                    }
                }
                else
                {
                    if (Input.IsActionJustPressed("NextSequenceStep"))
                    {
                        currentStepState = SequenceStepState.TryLoadNextStep;
                    }
                }
                break;
            case SequenceStepState.TryLoadNextStep:
                currentStepIndex++;
                if (currentStepIndex >= sequenceStepList.Count)
                {
                    //there's no next step so we stop playing
                    isPlaying = false;
                    currentStepState = SequenceStepState.None;
                    currentStep = null;
                }
                else
                {
                    //there is a next step and we go to that one
                    isPlaying = true;
                    currentStepState = SequenceStepState.StartStep;
                    currentStep = sequenceStepList[currentStepIndex];
                }
                break;
        }
    }
}
