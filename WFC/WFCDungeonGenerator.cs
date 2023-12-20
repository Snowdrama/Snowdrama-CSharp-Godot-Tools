using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;
using System.Reflection.Emit;

public partial class WFCDungeonGenerator : Node
{
    Stopwatch Stopwatch = new Stopwatch();
    Stopwatch Stopwatch2 = new Stopwatch();

    List<WFCTile> loadedTiles = new List<WFCTile>();
    
    Random rand = new Random();

    Dictionary<Vector2I, WFCNode> nodes;
    int mapWidth;
    int mapHeight;
    bool RemoveUnusedRegions = true;
    Stack<Vector2I> CompletedTiles = new Stack<Vector2I>();
    const int UNDO_COUNT = 16;
    public Dictionary<Vector2I, WFCNode> GenerateMap(string pieceFilePath, int mapWidth, int mapHeight, bool RemoveUnusedRegions = true, int seed = -1)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.RemoveUnusedRegions = RemoveUnusedRegions;
        this.nodes = StartGenerator(pieceFilePath, RemoveUnusedRegions, seed);

        CompletedTiles = new Stack<Vector2I>();
        GD.Print("Entrropy Before START ***********************************************************");
        OutputEntropyDebug();
        GD.Print("Entrropy Before END ***********************************************************");
        Stopwatch.Restart();
        int breakCount = 0;
        do
        {
            //update the Entropy until the Entropy is stable
            bool updateSuccess = UpdateGeneratorEntropy();
            //bool mapValid = CheckMapValidity();
            if (updateSuccess)
            {
                bool successful = ColapseLowestEntropy();
                if (!successful)
                {
                }
                breakCount++;
                if (CompletedTiles.Count == nodes.Values.Count)
                {
                    break;
                }
            }
            else
            {
                GD.PrintErr("SCREW IT start over!");
                GD.PrintErr("Entrropy Bad START ***********************************************************");
                OutputEntropyDebug();
                GD.PrintErr("Entrropy Bad END ***********************************************************");
                //SCREW IT start over!
                this.nodes = StartGenerator(pieceFilePath, RemoveUnusedRegions, seed + breakCount);
                CompletedTiles = new Stack<Vector2I>();

                // Reset the top X elements to have every tile again
                // Hoping that the tile that makes the map impossible is
                // one of the last UNDO_COUNT tiles.
                //List<Vector2I> elementsToUndo = new List<Vector2I>();
                //for (int i = 0; i < UNDO_COUNT; i++)
                //{
                //    if (CompletedTiles.Count > 0)
                //    {
                //        elementsToUndo.Add(CompletedTiles.Pop());
                //    }
                //}
                //UndoTiles(elementsToUndo);
            }


        } while (breakCount < 10000);
        Stopwatch.Stop();
        GD.Print($"Stopwatch Time: {Stopwatch.ElapsedMilliseconds}");
        GD.Print($"Loop Count: {breakCount}");

        GD.Print("Entropy After START ***********************************************************");
        OutputEntropyDebug();
        GD.Print("Entropy After END ***********************************************************");


        MarkRegionsAndRemoveSmallRegions(mapWidth, mapHeight, nodes);


        return nodes;
    }

    public void UndoTiles(List<Vector2I> listOfTilesToReset)
    {
        //we just reset these to have all possible Tiles again
        foreach (var position in listOfTilesToReset)
        {
            var node = new WFCNode(position);
            node.AddRange(loadedTiles);
            nodes[position] = node;
        }
    }

    private Dictionary<Vector2I, WFCNode> StartGenerator(string pieceFilePath, bool RemoveUnusedRegions = true, int seed = -1)
    {
        nodes = new Dictionary<Vector2I, WFCNode>();
        loadedTiles = LoadPieces(pieceFilePath);

        if (seed != -1)
        {
            rand = new Random(seed);
        }
        else
        {
            rand = new Random();
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var node = new WFCNode(new Vector2I(x, y));
                node.AddRange(loadedTiles);
                nodes.Add(node.Position, node);
            }
        }
        return nodes;
    }


    private bool UpdateGeneratorEntropy()
    {
        int innerBreak = 0;
        List<WFCNode> needsUpdating;
        bool updateSuccess;
        (needsUpdating, updateSuccess) = UpdateEntropy();

        if (!updateSuccess)
        {
            return false;
        }

        while (needsUpdating.Count > 0 && innerBreak < 10000)
        {
            (needsUpdating, updateSuccess) = UpdateEntropy(needsUpdating);

            if (!updateSuccess)
            {
                return false;
            }
            innerBreak++;
        }
        return true;
    }

    /// <summary>
    /// Check if any of the tiles have 0 entropy, which means the
    /// map can't be completed.
    /// </summary>
    /// <returns>False if the map can't be completed</returns>
    private bool CheckMapValidity()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var position = new Vector2I(x, y);
                if (nodes.ContainsKey(position))
                {
                    var node = nodes[position];
                    if (node.Entropy == 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool ColapseLowestEntropy()
    {
        int minEntropy = nodes.Values.Min(x => x.Entropy);
        List<WFCNode> nodesToUpdate = nodes.Values.Where(x => x.Entropy == minEntropy && !x.Colapsed).ToList();

        //we can't collapse if the node has no Tiles...
        if (nodesToUpdate.Count > 0)
        {
            //collapse the node
            var randomNode = nodesToUpdate[rand.Next(0, nodesToUpdate.Count)];
            randomNode.Colapse(rand);
            if (!CompletedTiles.Contains(randomNode.Position))
            {
                CompletedTiles.Push(randomNode.Position);
            }
            //return that the node Colapsed correctly.
            return true;
        }
        else
        {
            //the node failed to collapse correctly... <.< the whole map failed?
            //or we're done <.<
            return false;
        }
    }

    private (List<WFCNode>, bool) UpdateEntropy(List<WFCNode> nodesToUpdate = null)
    {
        if(nodesToUpdate == null)
        {
            nodesToUpdate = nodes.Values.Where(x => !x.Colapsed).OrderBy(x => x.Entropy).ToList();
        }

        List<WFCNode> newNodesToUpdate = new List<WFCNode>();
        foreach (var node in nodesToUpdate)
        {
            var changeInEntropy = node.UpdateEntropy(nodes);

            if (node.Entropy == 0)
            {
                return (null, false);
            }

            //addd this to a list of tiles to undo if we get stuck
            if (node.Colapsed && !CompletedTiles.Contains(node.Position))
            {
                CompletedTiles.Push(node.Position);
            }

            if (changeInEntropy > 0)
            {
                //since we updated the Entropy we need to add the neighbor nodes to the next update list
                if (nodes.ContainsKey(node.Position + new Vector2I(0, -1)))
                    newNodesToUpdate.Add(nodes[node.Position + new Vector2I(0, -1)]);
                if (nodes.ContainsKey(node.Position + new Vector2I(0, 1)))
                    newNodesToUpdate.Add(nodes[node.Position + new Vector2I(0, 1)]);
                if (nodes.ContainsKey(node.Position + new Vector2I(1, 0)))
                    newNodesToUpdate.Add(nodes[node.Position + new Vector2I(1, 0)]);
                if (nodes.ContainsKey(node.Position + new Vector2I(-1, 0)))
                    newNodesToUpdate.Add(nodes[node.Position + new Vector2I(-1, 0)]);
            }
        };

        return (newNodesToUpdate, true);
    }






























    public WFCTile GetTile(int index)
    {
        return loadedTiles[index];
    }

















    private void MarkRegionsAndRemoveSmallRegions(int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        int regionId = 0;

        int largestRegionId = int.MaxValue;
        int largestRegionCount = -1;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (nodes[new Vector2I(x, y)].RegionID == int.MaxValue)
                {
                    int regionSize = TraverseNodesAndMarkRegions(regionId, new Vector2I(x, y), 0, mapWidth, mapHeight, nodes);
                    if (regionSize > largestRegionCount)
                    {
                        largestRegionId = regionId;
                        largestRegionCount = regionSize;
                    }
                }
                regionId++;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (nodes[new Vector2I(x, y)].RegionID != largestRegionId)
                {
                    nodes[new Vector2I(x, y)] = null;
                }
            }
        }
    }

    private int TraverseNodesAndMarkRegions(int regionId, Vector2I position, int count, int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        if(position.X < 0 || position.Y < 0 || position.X >= mapWidth || position.Y >= mapHeight)
        {
            return count;
        }


        if (nodes[position].RegionID != int.MaxValue)
        {
            return count;
        }

        nodes[position].RegionID = regionId;
        count++;
        //look at node tile, see what directions we go in and then traverse to that node

        if (nodes[position].Tiles[0].ConnectionType_N != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(0, -1), count, mapWidth, mapHeight, nodes);
        }

        if (nodes[position].Tiles[0].ConnectionType_S != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(0, 1), count, mapWidth, mapHeight, nodes);
        }

        if (nodes[position].Tiles[0].ConnectionType_E != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(1, 0), count, mapWidth, mapHeight, nodes);
        }

        if (nodes[position].Tiles[0].ConnectionType_W != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(-1, 0), count, mapWidth, mapHeight, nodes);
        }
        return count;
    }


















    public static List<WFCTile> LoadPieces(string pieceFilePath)
    {
        using var dungeonGenPieces = FileAccess.Open(pieceFilePath, FileAccess.ModeFlags.Read);
        string dungeonGenPieceText = dungeonGenPieces.GetAsText(true);
        var data = JsonConvert.DeserializeObject<WFCTileContainer>(dungeonGenPieceText);

        List<WFCTile> uniqueTiles = new List<WFCTile>();
        foreach (var tile in data.JsonTiles)
        {
            //check the rotations and flip...
            //GD.Print($"Generating {tile.TileName} Tiles:");
            for (int i = 0; i <= tile.RotationCount; i++)
            {
                //GD.Print($"Generating {tile.TileName} Rotation {i}");
                if (tile.ChiralFlip)
                {
                    //GD.Print($"Generating {tile.TileName} Rotation {i} Flipped");
                    var newTileFlipped = tile.CreateTile(i, true);
                    uniqueTiles.Add(newTileFlipped);
                }
                var newTileNorm = tile.CreateTile(i, false);
                uniqueTiles.Add(newTileNorm);
            }
        }
        return uniqueTiles;
    }
























    public void OutputEntropyDebug()
    {
        string outputString = "";
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var en = nodes[new Vector2I(x, y)].Entropy;
                if (en == int.MaxValue)
                {
                    outputString += "XX | ";
                }
                else
                {
                    outputString += $"{en:00} | ";
                }
            }
            outputString += "\n";
        }

        GD.Print(outputString);
    }

    public void OutputRegionIdDebug()
    {
        string outputString = "";
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var node = nodes[new Vector2I(x, y)];
                if (node != null)
                {
                    if (node.RegionID == int.MaxValue)
                    {
                        outputString += "XX | ";
                    }
                    else
                    {
                        outputString += $"{node.RegionID:00} | ";
                    }
                }
                else
                {
                    outputString += "NN | ";
                }
            }
            outputString += "\n";
        }

        GD.Print(outputString);
    }

}
