using Newtonsoft.Json;

public class WFCTile
{
    //class so we can use as ref, readonly so we can't accidenally modify it
    [JsonProperty] public readonly string TileName;
    //the thing to load and spawn
    [JsonProperty] public readonly string SpawnResource;//can be a prefab tscn
    [JsonProperty] public readonly string DebugTextureResource;//must be a texture

    //this is an int to describe
    //a unique connection


    // public readonly int Rotation;
    // TODO: Idea: This could in theory return different
    // values depending on the rotation... <.<
    // maybe use a prop getter here? 
    [JsonProperty] public readonly int ConnectionType_N;
    [JsonProperty] public readonly int ConnectionType_S;
    [JsonProperty] public readonly int ConnectionType_E;
    [JsonProperty] public readonly int ConnectionType_W;



    [JsonConstructor]
    public WFCTile(string TileName, string SpawnResource, int ConnectionType_N, int ConnectionType_S, int ConnectionType_E, int ConnectionType_W)
    {
        this.TileName = TileName;
        this.SpawnResource = SpawnResource;
        this.ConnectionType_N = ConnectionType_N;
        this.ConnectionType_S = ConnectionType_S;
        this.ConnectionType_E = ConnectionType_E;
        this.ConnectionType_W = ConnectionType_W;
        // this.Rotation = Rotation;
    }

    // public WFCTile(WFCTile copyTile)
    // {
    //     TileName = copyTile.TileName;
    //     SpawnResource = copyTile.SpawnResource;
    //     ConnectionType_N = copyTile.ConnectionType_N;
    //     ConnectionType_S = copyTile.ConnectionType_S;
    //     ConnectionType_E = copyTile.ConnectionType_E;
    //     ConnectionType_W = copyTile.ConnectionType_W;
    // }

    public override string ToString()
    {
        return $"Tile:{TileName} N:{ConnectionType_N} | S:{ConnectionType_S} | E:{ConnectionType_E} | W:{ConnectionType_W}";
    }
}
