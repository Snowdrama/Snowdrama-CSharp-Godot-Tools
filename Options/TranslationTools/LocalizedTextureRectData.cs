using Godot;

[GlobalClass]
public partial class LocalizedTextureRectData : Resource
{
    [Export] public string code;
    [Export] public Texture2D texture;
}
