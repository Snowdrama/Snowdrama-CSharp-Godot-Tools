using Godot;

[GlobalClass]
public partial class CursorShapeData : Resource
{
    [Export] public Texture2D image;
    [Export] public Vector2 hotspot;
}
