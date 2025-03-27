using Godot;
using System;
using Godot.Collections;
public partial class MultiplayerEntitySpawner : MultiplayerSpawner
{
    [Export] Dictionary<String, PackedScene> Entities = new Dictionary<String, PackedScene>();

    Array<Enemy> enemies = new Array<Enemy>();
    
    public override void _Ready()
    {
        base._Ready();
        this.SpawnFunction = new Callable(this, nameof(SpawnEntity));
        if (!Multiplayer.IsServer())
        {
            return;
        }

        //TODO Remove Debug Spawn!
        if (enemies.Count <= 0)
        {
            Spawn(new Dictionary<string, Variant>()
            {
                { "name", "enemy_test" },
                { "position", Vector3.Zero },
            });

            Spawn(new Dictionary()
            {
                { "name", "enemy_test" },
                { "position", Vector3.Zero },
            });
        }

    }

    private Node SpawnEntity(Dictionary data)
    {
        var name = data["name"].AsString();
        var position = data["position"].AsVector3();

        Debug.Log($"Spawning Entity {name}");


        if (Entities.ContainsKey(name))
        {
            var newEntity = Entities[name].Instantiate();

            return newEntity;
        }
        else
        {
            Debug.LogError($"Entity {name} isn't in list of entities");
        }

        return null;
    }

    
    //public void RequestSpawnWave(WaveData waveToSpawn, Array<Vector3> waveSpawnPoints, Action<Array<Node>> thingSpawned)
    //{
    //    Array<Node> spawnedThingList = new Array<Node>();
    //    foreach (var entity in waveToSpawn)
    //    {
    //        var spawnedThing = Spawn();
    //        spawnedThingList.Add(spawnedThing);
    //    }
    //    thingSpawned?.Invoke(spawnedThingList);
    //}
}
