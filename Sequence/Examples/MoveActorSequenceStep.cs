using Godot;
using System;

public partial class MoveActorSequenceStep : SequenceNode
{
	[Export] Actor actor;

    [Export] Vector2 startPosition;
    [Export] Vector2 endPosition;
    [Export(PropertyHint.Range, "0.01, 100")] float lerpTime;
    float lerpAmount = 0.0f;
    float lerpSpeed
    {
        get
        {
            return 1 / lerpTime;
        }
    }

    public override void PlaySequence(Action setOnCompleted)
    {
        base.PlaySequence(setOnCompleted);
        lerpAmount = 0.0f;
        actor.Position = startPosition;
    }
    public override void _Process(double delta)
	{
        lerpAmount += (float)delta * lerpSpeed;
        actor.Position = startPosition.Lerp(endPosition, lerpTime);
    }
    public override void ForceComplete()
    {
        base.ForceComplete();
        lerpAmount = 1;
        actor.Position = startPosition.Lerp(endPosition, lerpTime);
    }
}
