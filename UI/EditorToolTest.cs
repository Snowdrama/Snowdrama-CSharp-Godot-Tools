using Godot;

[Tool]
public partial class EditorToolTest : Sprite2D
{
    public override void _Process(double delta)
    {
        Rotation += Mathf.Pi * (float)delta;
    }
}