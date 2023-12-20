using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class WFCNode
{
    public Vector2I Position;

    private LootTable<WFCTile> _chanceTiles = new LootTable<WFCTile>();
    private List<WFCTile> _tiles = new List<WFCTile>();
    public List<WFCTile> Tiles
    {
        get
        {
            return _tiles;
        }

        private set
        {
            _tiles = value;
            _allowedTiles_N = _tiles.Select(x => x.ConnectionType_N).ToList();
            _allowedTiles_S = _tiles.Select(x => x.ConnectionType_S).ToList();
            _allowedTiles_E = _tiles.Select(x => x.ConnectionType_E).ToList();
            _allowedTiles_W = _tiles.Select(x => x.ConnectionType_W).ToList();
        }
    }
    public int Entropy
	{
		get {

            if(Tiles.Count == 1)
            {
                return int.MaxValue;
            }

            return Tiles.Count; 
        }
	}
    public bool Colapsed
    {
        get
        {
            if (Tiles.Count == 1)
            {
                return true;
            }
            return false;
        }
    }
    
    public int RegionID = int.MaxValue;


    public WFCNode(Vector2I pos)
    {
        Position = pos;
        Tiles = new List<WFCTile>();
    }


    public void Add(WFCTile tile)
    {
        Tiles.Add(tile);
    }

    public void AddRange(List<WFCTile> tileList)
    {
        List<WFCTile> newTileList = new List<WFCTile>(Tiles);
        newTileList.AddRange(tileList);
        Tiles = newTileList;
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
    /// <returns>The number of Tiles changed by the Entropy update</returns>
    public int UpdateEntropy(Dictionary<Vector2I, WFCNode> nodes)
    {
        //filter NSEW nodes, if the dictionary doesn't contain a node for that direction, it's outside the map. 
        if(Entropy == 1)
        {
            return 0;
        }

        var positionN = Position + new Vector2I( 0, -1);
        var positionS = Position + new Vector2I( 0,  1);
        var positionE = Position + new Vector2I( 1,  0);
        var positionW = Position + new Vector2I(-1,  0);

        List<WFCTile> tempTileList = new List<WFCTile>(Tiles);

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

        int tileDifference = Tiles.Count - tempTileList.Count;
        if(tileDifference > 0)
        {
            Tiles = tempTileList;
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
        foreach (var tile in Tiles)
        {
            output += $"{tile}\n";
        }
        return output;
    }


    public void Colapse(Random rand)
    {
        if(Tiles.Count != 0)
        {
            int randomTile = rand.Next(0, Tiles.Count);
            //GD.Print($"Random Tile{randomTile} > {Tiles.Count}");
            Tiles = new List<WFCTile> { Tiles[randomTile] };
        }
    }
}
