using Godot;
using Godot.Collections;
using System;

public partial class MultiplayerLevelSpawner : MultiplayerSpawner
{
    [ExportCategory("Levels")]
    [Export] Dictionary<string, PackedScene> levels = new Dictionary<string, PackedScene>();

    Node currentLevel;
    public override void _Ready()
    {
        base._Ready();
        this.SpawnFunction = new Callable(this, nameof(SpawnLevel));

        if (!this.Multiplayer.IsServer())
        {
            return;
        }

        if(currentLevel == null)
        {
            Spawn("level_1");
        }
    }


    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void RequestLevelChange(string requestedLevel)
    {
        if(this.Multiplayer.IsServer())
        {
            Spawn(requestedLevel);
        }
    }

    public Node SpawnLevel(string levelName)
    {
        if (this.Multiplayer.IsServer() && currentLevel != null)
        {
            currentLevel.QueueFree();
        }

        if (levels.ContainsKey(levelName))
        {
            var newLevel = levels[levelName].Instantiate();
            currentLevel = newLevel;
            return newLevel;
        }
        else
        {
            Debug.Log($"Tried Loading Level {levelName} that is not valid");
        }
        return null;
    }
}
