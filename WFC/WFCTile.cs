using Newtonsoft.Json;
using System;

public class WFCTile
{
    //class so we can use as ref, readonly so we can't accidenally modify it
    public readonly string TileName;
    //the thing to load and spawn
    public readonly string SpawnResource;//can be a prefab tscn
    public readonly string DebugTextureResource;//must be a texture

    //this is an int to describe
    //a unique connection
    private readonly int[] Connections;
    public int ConnectionType_N { get { return Connections[0]; } }
    public int ConnectionType_S { get { return Connections[1]; } }
    public int ConnectionType_E { get { return Connections[2]; } }
    public int ConnectionType_W { get { return Connections[3]; } }

    //these are readonly but are NOT JSON PROPS
    public readonly int Rotation;
    public readonly bool Flip;

    public WFCTile(string TileName,
        string SpawnResource,
        string DebugTextureResource,
        int[] Connections,
        int Rotation = 0, 
        bool Flip = false
    )
    {
        this.TileName = TileName;
        this.SpawnResource = SpawnResource;
        this.DebugTextureResource = DebugTextureResource;
        this.Connections = Connections;
        this.Rotation = Rotation;
        this.Flip = Flip;
    }

    public WFCTile(WFCTile copyTile)
    {
        TileName = copyTile.TileName;
        TileName = copyTile.TileName;
        SpawnResource = copyTile.SpawnResource;
        Connections = copyTile.Connections;
        Rotation = copyTile.Rotation;
        Flip = copyTile.Flip;
    }

    public override string ToString()
    {
        return $"Tile:{TileName} N:{ConnectionType_N} | S:{ConnectionType_S} | E:{ConnectionType_E} | W:{ConnectionType_W} | Rotation: {Rotation} | Flip: {Flip}";
    }
}
