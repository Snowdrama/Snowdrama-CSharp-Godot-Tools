using Godot;
using Godot.Collections;

public partial class WaveData : Resource
{
    [Export] Array<PackedScene> entities = new Array<PackedScene>();
}