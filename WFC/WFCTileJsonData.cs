using Godot;
using Newtonsoft.Json;
using System;

public enum WFCTileType
{
    Compass4Way, //N/S/E/W directions
    Compass8Way, //N/S/E/W + NE/NW/SE/SW directions
    Voxel_6Way,  //X/Y/X +-1 = 6 directions
    Voxel_26Way, //3x3x3 cube = 27 with the center missing
}
public enum WFCTileIndex
{
    North = 0,
    South = 1,
    East =  2,
    West =  3,


    NorthEast = 4,
    NorthWest = 5,
    SouthEast = 6,
    SouthWest = 7,

    //voxel y positive
    YPos_North = 8,
    YPos_East = 9,
    YPos_South = 10,
    YPos_West = 11,
    YPos_Center = 12,
    YPos_NorthEast = 13,
    YPos_NorthWest = 14,
    YPos_SouthEast = 15,
    YPos_SouthWest = 16,

    //voxel y negative
    YNeg_North = 17,
    YNeg_East = 18,
    YNeg_South = 19,
    YNeg_West = 20,
    YNeg_Center = 21,
    YNeg_NorthEast = 22,
    YNeg_NorthWest = 23,
    YNeg_SouthEast = 24,
    YNeg_SouthWest = 25,
}
/// <summary>
/// This stores the data in terms of the JSON format, we then convert this to a regular WFCTile
/// after reading it from the file, including creating copies with rotated values.
/// </summary>
[System.Serializable]
public class WFCTileJsonData
{
    //class so we can use as ref, readonly so we can't accidenally modify it
    [JsonProperty] public readonly string TileName;

    //the thing to load and spawn
    [JsonProperty] public readonly string SpawnResource;//can be a prefab tscn
    [JsonProperty] public readonly string DebugTextureResource;//must be a texture

    [JsonProperty] public readonly WFCTileType TileType;
    //this is an int to describe
    //a unique connection

    /// <summary>
    /// PLEASE BE AWARE, this array has very specific notation for what indexes are which direction.
    /// 
    /// The indexes are listed in the WFCTileIndex enum.
    /// </summary>
    [JsonProperty] private int[] Connections;


    // this is how many copies to make
    // and times to "rotate" it on load
    [JsonProperty] public readonly int RotationCount;

    // does flipping it mean it creates unique rotations?
    [JsonProperty] public readonly bool ChiralFlip;

    [JsonConstructor]
    public WFCTileJsonData(string TileName,
        string SpawnResource,
        string DebugTextureResource,
        int[] Connections,
        int RotationCount = 0,
        bool ChiralFlip = false)
    {
        this.TileName = TileName;
        this.SpawnResource = SpawnResource;
        this.DebugTextureResource = DebugTextureResource;
        this.Connections = Connections;
        this.RotationCount = RotationCount;
        this.ChiralFlip = ChiralFlip;
    }

    public WFCTile CreateTile(int rotation, bool flip)
    {
        int[] rotatedConnections = new int[0];
        switch (TileType)
        {
            case WFCTileType.Compass4Way:
                rotatedConnections = GetRotatedIndexes4Way(rotation, flip);
                break;
            default:
                throw new Exception($"The Tile Type of {TileType} is not currently supported");
        }

        WFCTile newTile = new WFCTile(
            TileName,
            SpawnResource,
            DebugTextureResource,
            rotatedConnections,
            rotation,
            flip);

        return newTile;
    }

    private int[] GetRotatedIndexes4Way(int rotation, bool flip)
    {
        int[] rotatiedConnections = new int[Connections.Length];

        switch (rotation)
        {
            case 0:
                if (flip)
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.North];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.South];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.West];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.East];
                }
                else
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.North];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.South];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.East];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.West];
                }
                break;
            case 1:
                if (flip)
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.East];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.West];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.North];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.South];
                }
                else
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.West];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.East];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.North];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.South];
                }
                break;
            case 2:
                if (flip)
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.South];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.North];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.East];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.West];
                }
                else
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.South];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.North];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.West];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.East];
                }
                break;
            case 3:
                if (flip)
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.West];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.East];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.South];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.North];
                }
                else
                {
                    rotatiedConnections[(int)WFCTileIndex.North] = Connections[(int)WFCTileIndex.East];
                    rotatiedConnections[(int)WFCTileIndex.South] = Connections[(int)WFCTileIndex.West];
                    rotatiedConnections[(int)WFCTileIndex.East] = Connections[(int)WFCTileIndex.South];
                    rotatiedConnections[(int)WFCTileIndex.West] = Connections[(int)WFCTileIndex.North];
                }
                break;
        }
        return rotatiedConnections;
    }
}
