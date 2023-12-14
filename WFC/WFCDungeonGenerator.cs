using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.Common;
using System.Linq;

public partial class WFCDungeonGenerator : Node
{
    public static List<WFCTile> LoadPieces()
    {
        using var dungeonGenPieces = FileAccess.Open("res://WFCTiles.json", FileAccess.ModeFlags.Read);
        string dungeonGenPieceText = dungeonGenPieces.GetAsText(true);
        var data = JsonConvert.DeserializeObject<WFCTileContainer>(dungeonGenPieceText);
        return data.Tiles;
    }


    public Dictionary<Vector2I, WFCNode> GenerateMap(int mapWidth, int mapHeight, int seed = -1)
    {

        List<WFCTile> loadedTileData = new List<WFCTile>();
        Dictionary<Vector2I, WFCNode> nodes = new Dictionary<Vector2I, WFCNode>();
        //create a new random

        loadedTileData = LoadPieces();

        Random rand;
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
        OutputEntropyDebug(mapWidth, mapHeight, nodes);

        //generate the map
        int unfinishedTiles = int.MaxValue;
        int breakCount = 10000;
        do
        {
            //if the entropy was updated in the last phase
            //we to update the entropy until the map becomes
            //stable
            bool wasUpdated = false;
            do
            {
                wasUpdated = UpdateEntropy(nodes);
            } while (wasUpdated);
            
            //collapse a random node with the lowest entropy 
            CollapseLowestEntropy(rand, nodes);
            //break count prevents infinite loops
            breakCount--;
            //if we have unfinished tiles(tiles with > 1 entropy)
            //we want to continue to update and collapse tiles
            unfinishedTiles = nodes.Values.Where(x => x.entropy != int.MaxValue).Count();
        } while (unfinishedTiles > 0 || breakCount == 0);

        //finally we need to clean up the level and prune any sections
        //that aren't contiguous with the main section
        MarkRegionsAndRemoveSmallRegions(mapWidth, mapHeight, nodes);
        //OutputRegionIdDebug();
        //OutputEntropyDebug();
        //OutputDebugTextures();

        return nodes;
    }
    public void CollapseLowestEntropy(Random localRand, Dictionary<Vector2I, WFCNode> nodes)
    {
        int minEntropy = nodes.Values.Min(x => x.entropy);
        List<WFCNode> nodesToUpdate = nodes.Values.Where(x => x.entropy == minEntropy).ToList();
        var randomNode = nodesToUpdate[localRand.Next(0, nodesToUpdate.Count)];

        randomNode.Collapse(localRand);
    }

    public bool UpdateEntropy(Dictionary<Vector2I, WFCNode> nodes)
    {
        bool wasUpdated = false;

        List<WFCNode> nodesToUpdate = nodes.Values.OrderBy(x => x.entropy).ToList();

        foreach (var node in nodesToUpdate)
        {
            if (node.UpdateEntropy(nodes) > 0)
            {
                wasUpdated = true;
            }
        };

        return wasUpdated;
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
                    int regionSize = TraverseNodesAndMarkRegions(regionId, new Vector2I(x, y), 0, nodes);
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

    public int TraverseNodesAndMarkRegions(int regionId, Vector2I position, int count, Dictionary<Vector2I, WFCNode> nodes)
    {
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
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(0, -1), count, nodes);
        }

        if (nodes[position].tiles[0].ConnectionType_S != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(0, 1), count, nodes);
        }

        if (nodes[position].tiles[0].ConnectionType_E != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(1, 0), count, nodes);
        }

        if (nodes[position].tiles[0].ConnectionType_W != 0)
        {
            //walk up
            count = TraverseNodesAndMarkRegions(regionId, position + new Vector2I(-1, 0), count, nodes);
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
