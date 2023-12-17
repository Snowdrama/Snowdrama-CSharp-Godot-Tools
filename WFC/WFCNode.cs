using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class WFCNode
{
    public Vector2I Position;
    public List<WFCTile> _tiles = new List<WFCTile>();
    public List<WFCTile> tiles
    {
        get
        {
            return _tiles;
        }

        set
        {
            _tiles = value;
            _allowedTiles_N = tiles.Select(x => x.ConnectionType_N).ToList();
            _allowedTiles_S = tiles.Select(x => x.ConnectionType_S).ToList();
            _allowedTiles_E = tiles.Select(x => x.ConnectionType_E).ToList();
            _allowedTiles_W = tiles.Select(x => x.ConnectionType_W).ToList();
        }
    }
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
        List<WFCTile> newTileList = new List<WFCTile>(tiles);
        newTileList.AddRange(tileList);
        tiles = newTileList;
    }
    //the allowed Tiles to the south of this, are all the ones with
    //a north connection that matches...? I think... I'm tired.

    //I could cache this... <.< lol
    //TODO: Cache this when the tile list changes

    private List<int> _allowedTiles_N;
    private List<int> _allowedTiles_S;
    private List<int> _allowedTiles_E;
    private List<int> _allowedTiles_W;
    public List<int> allowedTiles_N
    {
        get
        {
            return _allowedTiles_N;
        }
    }
    public List<int> allowedTiles_S
    {
        get
        {
            return _allowedTiles_S;
        }
    }
    public List<int> allowedTiles_E
    {
        get
        {
            return _allowedTiles_E;
        }
    }
    public List<int> allowedTiles_W
    {
        get
        {
            return _allowedTiles_W;
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

        var TileN_allowedTiles_S = (nodes.ContainsKey(positionN)) ? nodes[positionN].allowedTiles_S : new List<int>() { 0 };
        var TileS_allowedTiles_N = (nodes.ContainsKey(positionS)) ? nodes[positionS].allowedTiles_N : new List<int>() { 0 };
        var TileE_allowedTiles_W = (nodes.ContainsKey(positionE)) ? nodes[positionE].allowedTiles_W : new List<int>() { 0 };
        var TileW_allowedTiles_E = (nodes.ContainsKey(positionW)) ? nodes[positionW].allowedTiles_E : new List<int>() { 0 };


        //filter
        tempTileList = tempTileList.Where(x =>
            TileN_allowedTiles_S.Contains(x.ConnectionType_N) &&
            TileS_allowedTiles_N.Contains(x.ConnectionType_S) &&
            TileE_allowedTiles_W.Contains(x.ConnectionType_E) &&
            TileW_allowedTiles_E.Contains(x.ConnectionType_W)
        ).ToList();

        int tileDifference = tiles.Count - tempTileList.Count;
        if(tileDifference > 0)
        {
            tiles = tempTileList;
        }

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
        if(tiles.Count != 0)
        {
            int randomTile = rand.Next(0, tiles.Count);
            //GD.Print($"Random Tile{randomTile} > {tiles.Count}");
            tiles = new List<WFCTile> { tiles[randomTile] };
        }
    }
}
