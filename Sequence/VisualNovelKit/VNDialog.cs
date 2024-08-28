using Godot;
using Godot.Collections;

public struct DialogInfo
{

    public string dialog;
}

public class SayDialog : AMessage<VNActorData, string> { }
public partial class VNDialog : Node
{
    [Export] RichTextLabel textField;

    [Export] Label nameLabel;

    [Export] TextureRect[] characterImages;

    [Export] Array<VNActor> actorPool = new Array<VNActor>();

    Dictionary<string, VNActor> assignedActor = new Dictionary<string, VNActor>();

    [Export] Array<Control> positionList = new Array<Control>();
    public void AddActor(VNActorData actorData)
    {
        var actor = actorPool[0];
        actorPool.RemoveAt(0);
        actor.SetActorData(actorData);
        assignedActor.Add(actorData.CharacterName, actor);
    }

    public void SayDialog(VNActorData actor, string dialog)
    {
        nameLabel.Text = actor.CharacterName;
        textField.Text = dialog;
    }

    public void SetActorDetails(VNActorData actor, int positionIndex, bool flipHorizontal)
    {

    }
}