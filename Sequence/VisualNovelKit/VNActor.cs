using Godot;

public partial class VNActor : Control
{
    [Export]TextureRect actorTexture;

    [Export] Control positionTarget;
    VNActorData actorData;

    public override void _EnterTree()
    {
        this.AddToGroup("Actor");
    }


    public void SetActorData(VNActorData data)
    {
        actorData = data;
        actorTexture.Texture = data.DefaultCharacterImage;
    }

    public void SetEmote(string emoteName)
    {

    }

    public void SetFacingDirection(bool flipHorizontal)
    {

    }

    public override void _Process(double delta)
    {
        actorTexture.Position = positionTarget.Position;




    }
}
