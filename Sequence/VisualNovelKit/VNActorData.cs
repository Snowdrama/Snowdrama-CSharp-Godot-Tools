using Godot;

public partial class VNActorData : Resource
{
    [Export] string _characterName;
    public string CharacterName { get { return _characterName; } }

    [Export] Texture2D _defaultCharacterIamge;
    public Texture2D DefaultCharacterImage { get {  return _defaultCharacterIamge; } }
}
