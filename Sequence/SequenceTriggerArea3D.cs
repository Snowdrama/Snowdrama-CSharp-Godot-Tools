using Godot;

public partial class SequenceTriggerArea3D : Area3D
{
    [Export(PropertyHint.NodeType, "Sequence")] Sequence sequenceToTrigger;
    public override void _EnterTree()
    {
        this.BodyEntered += SequenceTriggerArea3D_BodyEntered;
    }

    private void SequenceTriggerArea3D_BodyEntered(Node3D body)
    {
        sequenceToTrigger.StartSequence();
    }
}