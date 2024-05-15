using Godot;
using Godot.Collections;
using System;


/// <summary>
/// Plays multiple sequence steps together in paralell 
/// and then only is complete once every child step is compltete
/// </summary>
public partial class SequenceGroup : SequenceNode
{
    [Export] int childrenComplete = 0;
    [Export] Array<SequenceNode> chidlren;

    public override void PlaySequence(Action setOnCompleted)
    {
        childrenComplete = 0;
        base.PlaySequence(setOnCompleted);
        foreach (var item in chidlren)
        {
            //we want to know when a child completes
            item.PlaySequence(OnChildCompleted);
        }
        this.State = SequenceState.Playing;
    }

    private void OnChildCompleted()
    {
        childrenComplete++;
        if(chidlren.Count == childrenComplete)
        {
            this.Completed();
        }
    }

    public override void ForceComplete()
    {
        base.ForceComplete();
        foreach (var item in chidlren)
        {
            //all children complete immediately
            item.ForceComplete();
        }
    }
}
