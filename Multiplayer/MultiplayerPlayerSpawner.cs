using Godot;
using Godot.Collections;

[GlobalClass]
public partial class MultiplayerPlayerSpawner : MultiplayerSpawner
{
    [Export] PackedScene playerScene;
    [Export] Array<Marker3D> spawn_positions = new Array<Marker3D>();
    Vector3[] spawns;

    Dictionary<int, Player3D> players = new Dictionary<int, Player3D>();
    public override void _Ready()
    {
        base._Ready();
        SpawnFunction = new Callable(this, nameof(AddPlayer));

        spawns = new Vector3[spawn_positions.Count];
        for (int i = 0; i < spawn_positions.Count; i++)
        {
            spawns[i] = spawn_positions[i].GlobalPosition;
        }

        if(!Multiplayer.IsServer())
        {
            return;
        }

        Multiplayer.PeerConnected += SpawnPlayer;
        Multiplayer.PeerDisconnected += SpawnPlayer;

        foreach (var id in Multiplayer.GetPeers())
        {
            Spawn(id);
        }

        //spawn the host if not a dedicated server
        if (!OS.HasFeature("dedicated_server"))
        {
            Spawn(1);
        }
    }
    private void SpawnPlayer(long id)
    {
        if (!Multiplayer.IsServer())
        {
            return;
        }
        Spawn(id);
    }
    private void RemovePlayer(long id)
    {
        if (!Multiplayer.IsServer())
        {
            return;
        }
        if (players.ContainsKey((int)id))
        {
            players[(int)id].QueueFree();
            players.Remove((int)id);
        }
    }
    private Node AddPlayer(int id)
    {
        if (players.ContainsKey(id))
        {
            Debug.LogError($"Player with id {id} already exists can't spawn");
            return null;
        }

        var newPlayer = (Player3D)playerScene.Instantiate();
        newPlayer.Name = $"Player_{id}";
        newPlayer.SetMultiplayerAuthority(id);
        newPlayer.SetId(id);
        newPlayer.SetSpawn(spawns.GetRandom());
        players.Add(id, newPlayer);
        return newPlayer;
    }
}
