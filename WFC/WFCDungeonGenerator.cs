using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.Common;
using System.Linq;
using System.Diagnostics;
using static Godot.OpenXRInterface;

public partial class WFCDungeonGenerator : Node
{
    Stopwatch Stopwatch = new Stopwatch();
    Stopwatch Stopwatch2 = new Stopwatch();
    List<WFCTile> loadedTiles = new List<WFCTile>();
    Random rand = new Random();
    public static List<WFCTile> LoadPieces(string pieceFilePath)
    {
        using var dungeonGenPieces = FileAccess.Open(pieceFilePath, FileAccess.ModeFlags.Read);
        string dungeonGenPieceText = dungeonGenPieces.GetAsText(true);
        var data = JsonConvert.DeserializeObject<WFCTileContainer>(dungeonGenPieceText);

        List<WFCTile> uniqueTiles = new List<WFCTile>();
        foreach (var tile in data.JsonTiles)
        {
            //check the rotations and flip...
            //GD.Print($"Generating {tile.TileName} tiles:");
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

    public WFCTile GetTile(int index)
    {
        return loadedTiles[index];
    }

    public Dictionary<Vector2I, WFCNode> GenerateMap(string pieceFilePath, int mapWidth, int mapHeight, bool RemoveUnusedRegions = true, int seed = -1)
    {

        List<WFCTile> loadedTileData = new List<WFCTile>();
        Dictionary<Vector2I, WFCNode> nodes = new Dictionary<Vector2I, WFCNode>();
        //create a new random

        loadedTileData = LoadPieces(pieceFilePath);
        loadedTiles = loadedTileData;

        if (seed != -1)
        {
            rand = new Random(seed);
        }
        else
        {
            rand = new Random();
        }

        //clear any previous map
        nodes.Clear();



        //initialize the nodes in the new map;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var node = new WFCNode(new Vector2I(x, y));
                node.AddRange(loadedTileData);

                nodes.Add(node.Position, node);
            }
        }
        //OutputEntropyDebug(mapWidth, mapHeight, nodes);
        //generate the map
        int unfinishedTiles = int.MaxValue;
        int breakCount = 0;
        do
        {
            int innerBreak = 0;
            List<WFCNode> needsUpdating = UpdateEntropyFirstTime(nodes);
            while (needsUpdating.Count > 0 && innerBreak < 1000)
            {
                //Stopwatch2.Restart();
                needsUpdating = UpdateEntropy(needsUpdating, nodes);
                innerBreak++;
            }

            CollapseLowestEntropy(rand, nodes);
            breakCount++;
            unfinishedTiles = nodes.Values.Where(x => x.entropy != int.MaxValue).Count();
        } while (unfinishedTiles > 0 && breakCount <= 1000);

        //finally we need to clean up the level and prune any sections
        //that aren't contiguous with the main section
        //if (RemoveUnusedRegions)
        //{
        //    MarkRegionsAndRemoveSmallRegions(mapWidth, mapHeight, nodes);
        //}

        return nodes;
    }

    public Dictionary<Vector2I, WFCNode> StartGenerator(string pieceFilePath, int mapWidth, int mapHeight, bool RemoveUnusedRegions = true, int seed = -1)
    {
        Dictionary<Vector2I, WFCNode> nodes = new Dictionary<Vector2I, WFCNode>();
        //create a new random

        List<WFCTile> loadedTileData = LoadPieces(pieceFilePath);
        loadedTiles = loadedTileData;

        if (seed != -1)
        {
            rand = new Random(seed);
        }
        else
        {
            rand = new Random();
        }
        //clear any previous map
        nodes.Clear();
        //initialize the nodes in the new map;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var node = new WFCNode(new Vector2I(x, y));
                node.AddRange(loadedTileData);

                nodes.Add(node.Position, node);
            }
        }
        //OutputEntropyDebug(mapWidth, mapHeight, nodes);

        return nodes;
    }

    public Dictionary<Vector2I, WFCNode> UpdateGenerator(Dictionary<Vector2I, WFCNode> nodes, int mapWidth, int mapHeight)
    {
        int innerBreak = 0;
        List<WFCNode> needsUpdating = UpdateEntropyFirstTime(nodes);
        while (needsUpdating.Count > 0 && innerBreak < 1000)
        {
            //Stopwatch2.Restart();
            needsUpdating = UpdateEntropy(needsUpdating, nodes);
            innerBreak++;
        }
        //OutputEntropyDebug(mapWidth, mapHeight, nodes);
        return nodes;
    }

    public (Dictionary<Vector2I, WFCNode>, bool) CollapseNode(Dictionary<Vector2I, WFCNode> nodes, int mapWidth, int mapHeight)
    {
        bool didCollapse = CollapseLowestEntropy(rand, nodes);
        //OutputEntropyDebug(mapWidth, mapHeight, nodes);
        return (nodes, didCollapse);
    }



    public bool CollapseLowestEntropy(Random localRand, Dictionary<Vector2I, WFCNode> nodes)
    {
        int minEntropy = nodes.Values.Min(x => x.entropy);
        List<WFCNode> nodesToUpdate = nodes.Values.Where(x => x.entropy == minEntropy && x.entropy != int.MaxValue).ToList();
        if (nodesToUpdate.Count > 0)
        {
            var randomNode = nodesToUpdate[localRand.Next(0, nodesToUpdate.Count)];
            randomNode.Collapse(localRand);
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<WFCNode> UpdateEntropy(List<WFCNode> nodeList, Dictionary<Vector2I, WFCNode> nodeMap)
    {
        List<WFCNode> nodesToUpdate = nodeList.OrderBy(x => x.entropy).ToList();

        List<WFCNode> newNodesToUpdate = new List<WFCNode>();
        foreach (var node in nodesToUpdate)
        {
            if (node.UpdateEntropy(nodeMap) > 0)
            {
                if (nodeMap.ContainsKey(node.Position + new Vector2I(0, -1)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(0, -1)]);
                if (nodeMap.ContainsKey(node.Position + new Vector2I(0, 1)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(0, 1)]);
                if (nodeMap.ContainsKey(node.Position + new Vector2I(1, 0)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(1, 0)]);
                if (nodeMap.ContainsKey(node.Position + new Vector2I(-1, 0)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(-1, 0)]);
            }
        };

        return newNodesToUpdate;
    }

    public List<WFCNode> UpdateEntropyFirstTime(Dictionary<Vector2I, WFCNode> nodeMap)
    {
        List<WFCNode> nodesToUpdate = nodeMap.Values.Where(x => x.entropy != int.MaxValue).OrderBy(x => x.entropy).ToList();

        List<WFCNode> newNodesToUpdate = new List<WFCNode>();
        foreach (var node in nodesToUpdate)
        {
            //if we updated this node add the children to update
            if (node.UpdateEntropy(nodeMap) > 0)
            {
                if(nodeMap.ContainsKey(node.Position + new Vector2I(0, -1)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(0, -1)]);
                if (nodeMap.ContainsKey(node.Position + new Vector2I(0, 1)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(0, 1)]);
                if (nodeMap.ContainsKey(node.Position + new Vector2I(1, 0)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(1, 0)]);
                if (nodeMap.ContainsKey(node.Position + new Vector2I(-1, 0)))
                    newNodesToUpdate.Add(nodeMap[node.Position + new Vector2I(-1, 0)]);
            }
        };

        return newNodesToUpdate;
    }

    public void MarkRegionsAndRemoveSmallRegions(int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        int regionId = 0;

        int largestRegionId = int.MaxValue;
        int largestRegionCount = -1;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (nodes[new Vector2I(x, y)].regionID == int.MaxValue)
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
                if (nodes[new Vector2I(x, y)].regionID != largestRegionId)
                {
                    nodes[new Vector2I(x, y)] = null;
                }
            }
        }
    }

    public int TraverseNodesAndMarkRegions(int regionId, Vector2I position, int count, int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        if(position.X < 0 || position.Y < 0 || position.X >= mapWidth || position.Y >= mapHeight)
        {
            return count;
        }


        if (nodes[position].regionID != int.MaxValue)
        {
            return count;
        }

        nodes[position].regionID = regionId;
        count++;
        //look at node tile, see what directions we go in and then traverse to that node

        if (nodes[position].tiles[0].ConnectionType_N != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(0, -1), count, mapWidth, mapHeight, nodes);
        }

        if (nodes[position].tiles[0].ConnectionType_S != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(0, 1), count, mapWidth, mapHeight, nodes);
        }

        if (nodes[position].tiles[0].ConnectionType_E != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(1, 0), count, mapWidth, mapHeight, nodes);
        }

        if (nodes[position].tiles[0].ConnectionType_W != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(-1, 0), count, mapWidth, mapHeight, nodes);
        }
        return count;
    }
















    public void OutputEntropyDebug(int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        string outputString = "";
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var en = nodes[new Vector2I(x, y)].entropy;
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

    public void OutputRegionIdDebug(int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        string outputString = "";
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var node = nodes[new Vector2I(x, y)];
                if (node != null)
                {
                    if (node.regionID == int.MaxValue)
                    {
                        outputString += "XX | ";
                    }
                    else
                    {
                        outputString += $"{node.regionID:00} | ";
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
