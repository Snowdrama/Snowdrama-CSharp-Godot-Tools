using Godot;
using System;

public partial class LevelSwitchExample : Node
{
    [Export] MultiplayerLevelSpawner spawner;
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsKeyPressed(Key.Key1))
        {
            spawner.Rpc(nameof(spawner.RequestLevelChange), "level_1");
        }
        if (Input.IsKeyPressed(Key.Key2))
        {
            spawner.Rpc(nameof(spawner.RequestLevelChange), "level_2");
        }
    }
}
