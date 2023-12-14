using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class WFCNode
{
    public Vector2I Position;
	public List<WFCTile> tiles = new List<WFCTile>();
	public int entropy
	{
		get { 
            if(tiles.Count == 1)
            {
                return int.MaxValue;
            }

            return tiles.Count; 
        }
	}
    
    public int regionID = int.MaxValue;


    public WFCNode(Vector2I pos)
    {
        Position = pos;
        tiles = new List<WFCTile>();
    }


    public void Add(WFCTile tile)
    {
        tiles.Add(tile);
    }

    public void AddRange(List<WFCTile> tileList)
    {
        tiles.AddRange(tileList);

        //TODO: update cached connections for allowed Tiles
    }
    //the allowed Tiles to the south of this, are all the ones with
    //a north connection that matches...? I think... I'm tired.

    //I could cache this... <.< lol
    //TODO: Cache this when the tile list changes
    public List<int> allowedTiles_N
    {
        get
        {
            // TODO: Idea: This could in theory return different
            // arrays depending on the rotation... <.< 
            return tiles.Select(x => x.ConnectionType_N).ToList();
        }
    }
    public List<int> allowedTiles_S
    {
        get
        {
            return tiles.Select(x => x.ConnectionType_S).ToList();
        }
    }
    public List<int> allowedTiles_E
    {
        get
        {
            return tiles.Select(x => x.ConnectionType_E).ToList();
        }
    }
    public List<int> allowedTiles_W
    {
        get
        {
            return tiles.Select(x => x.ConnectionType_W).ToList();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns>The number of tiles changed by the entropy update</returns>
    public int UpdateEntropy(Dictionary<Vector2I, WFCNode> nodes)
    {
        //filter NSEW nodes, if the dictionary doesn't contain a node for that direction, it's outside the map. 
        if(entropy == 1)
        {
            return 0;
        }

        var positionN = Position + new Vector2I( 0, -1);
        var positionS = Position + new Vector2I( 0,  1);
        var positionE = Position + new Vector2I( 1,  0);
        var positionW = Position + new Vector2I(-1,  0);

        List<WFCTile> tempTileList = new List<WFCTile>(tiles);

        if (!nodes.ContainsKey(positionN))
        {
            tempTileList = tempTileList.Where(x => x.ConnectionType_N == 0).ToList();
        }
        else
        {
            tempTileList = tempTileList.Where(x => nodes[positionN].allowedTiles_S.Contains(x.ConnectionType_N)).ToList();
        }

        if (!nodes.ContainsKey(positionS))
        {
            tempTileList = tempTileList.Where(x => x.ConnectionType_S == 0).ToList();
        }
        else
        {
            tempTileList = tempTileList.Where(x => nodes[positionS].allowedTiles_N.Contains(x.ConnectionType_S)).ToList();
        }

        if (!nodes.ContainsKey(positionE))
        {
            tempTileList = tempTileList.Where(x => x.ConnectionType_E == 0).ToList();
        }
        else
        {
            tempTileList = tempTileList.Where(x => nodes[positionE].allowedTiles_W.Contains(x.ConnectionType_E)).ToList();
        }

        if (!nodes.ContainsKey(positionW))
        {
            tempTileList = tempTileList.Where(x => x.ConnectionType_W == 0).ToList();
        }
        else
        {
            tempTileList = tempTileList.Where(x => nodes[positionW].allowedTiles_E.Contains(x.ConnectionType_W)).ToList();
        }

        int tileDifference = tiles.Count - tempTileList.Count;

        tiles = tempTileList;
        return tileDifference;
    }

    public override string ToString()
    {
        string output = $"*** WFC Tile: {this.Position} ***\n";

        var positionN = Position + new Vector2I(0, -1);
        var positionS = Position + new Vector2I(0, 1);
        var positionE = Position + new Vector2I(1, 0);
        var positionW = Position + new Vector2I(-1, 0);
        output += $"N Pos: {positionN}\n";
        output += $"S Pos: {positionS}\n";
        output += $"E Pos: {positionE}\n";
        output += $"W Pos: {positionW}\n";
        foreach (var tile in tiles)
        {
            output += $"{tile}\n";
        }
        return output;
    }


    public void Collapse(Random rand)
    {
        int randomTile = rand.Next(0, tiles.Count);
        tiles = new List<WFCTile> { tiles[randomTile] };
    }
}
