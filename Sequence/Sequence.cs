using Godot;
using System;
using System.Collections.Generic;
public partial class Sequence : Node, ISequence
{
    [Export]Godot.Collections.Array<Control> usedContainers = new Godot.Collections.Array<Control>();
    [Export]Godot.Collections.Array<Node2D> usedNode2Ds = new Godot.Collections.Array<Node2D>();

    List<SequenceNode> sequenceNodes = new List<SequenceNode>();
    SequenceNode currentStep;
    int currentStepIndex = 0;
    public SequenceState State { get; private set; }

    [Export] bool AutoAdvance = false;

    [Export] bool CanPlayMoreThanOnce = false; 
    public override void _Ready()
	{
		for (int i = 0; i < this.GetChildCount(); i++)
		{
			var child = this.GetChild(i);
			if(child is SequenceNode sn)
			{
				sequenceNodes.Add(sn);
			}
        }


        //var allChildren = new List<Node>();
        //GetAllChildren(this, ref allChildren);
        //foreach (var item in allChildren)
        //{
        //    GD.Print(item.Name);
        //    if(item is Control c)
        //    {
        //        usedContainers.Add(c);
        //    }
        //    else if(item is Node2D n2d)
        //    {
        //        usedNode2Ds.Add(n2d);
        //    }
        //}

        foreach (var item in usedNode2Ds)
        {
            item.Hide();
        }

        foreach (var item in usedContainers)
        {
            item.Hide();
        }
    }

    private void GetAllChildren(Node node, ref List<Node> allChildren)
    {
        foreach (var child in node.GetChildren())
        {
            allChildren.Add(child);
            if(child.GetChildCount() > 0)
            {
                GetAllChildren(child, ref allChildren);
            }
        }
    }

	public override void _Process(double delta)
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

    public void PlaySequence(bool autoAdvance = false)
    {
        if(State == SequenceState.Stopped)
        {
            //we are starting so the index is -1
            currentStepIndex = -1;
            LoadNextStep();
        }
    }

    public void AdvanceSequence()
    {
        //we've interacted with the game so we want to advance the sequence
        if (currentStep != null)
        {
            //Are we already waiting for input? if so load the next step
            if (currentStep.State == SequenceState.Completed)
            {
                LoadNextStep();
            }
            //We're still running so we're going to force complete skipping the animations or dialog
            else
            {
                currentStep.ForceComplete();
            }
        }
    }

    public void LoadNextStep()
    {
        if(currentStep != null)
        {
            //make sure the animations are done and everything
            //is at the target
            currentStep.ForceComplete();

            //unload anything that needs unloading
            currentStep.UnloadSequence();
            State = SequenceState.Stopped;
        }
        //were going to the next step
        currentStepIndex++;

        //make sure we have a step available
        if(currentStepIndex >= sequenceNodes.Count)
        {
            //we're out of steps!
            GD.PrintErr("Out of steps!");
            State = SequenceState.Stopped;
        }
        else
        {
            //we have a step to process
            currentStep = sequenceNodes[currentStepIndex];

            //load stuff before starting
            currentStep.LoadSequence();

            //then start playing the sequence
            currentStep.PlaySequence();
            State = SequenceState.Playing;
        }
    }

    public void PauseSequence()
    {
    }

    public void StopSequence()
    {

    }
}
