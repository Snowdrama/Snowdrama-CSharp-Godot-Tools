using Godot;
using System;
using System.Collections.Generic;

public partial class WFCSpawnNodePrefabs : Node
{
    [Export]
    WFCDungeonGenerator generator;

    [Export]
    int mapWidth = 32;
    [Export]
    int mapHeight = 32;

    [Export]
    Vector2 prefabSize = new Vector2(20, 20);

    Dictionary<Vector2I, WFCNode> generatedNodes;
    Dictionary<string, PackedScene> loadedScenePrefabs = new Dictionary<string, PackedScene>();
    List<Node> nodeList = new List<Node>();
    int debugTileW = 32;
    int debugTileH = 32;


    public override void _Ready()
    {
        
    }

    public Dictionary<Vector2I, WFCNode> GenerateMap()
    {
        generatedNodes = generator.GenerateMap(mapWidth, mapHeight);
        SpawnPrefabs(mapWidth, mapHeight, generatedNodes);

        return generatedNodes;
    }

    private void SpawnPrefabs(int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        foreach (var item in nodeList)
        {
            item.QueueFree();
        }
        nodeList.Clear();

        string outputString = "";
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var nodePosition = new Vector2I(x, y);
                var tempNode = nodes[nodePosition];
                if (tempNode != null)
                {
                    var prefabLocation = nodes[nodePosition].tiles[0].SpawnResource;

                    if (!loadedScenePrefabs.ContainsKey(prefabLocation))
                    {
                        loadedScenePrefabs.Add(prefabLocation, ResourceLoader.Load<PackedScene>(prefabLocation));;
                    }

                    //HMMMMMM
                    GD.Print(prefabLocation);
                    var instantiatedPrefab = loadedScenePrefabs[prefabLocation].Instantiate();
                    if(instantiatedPrefab is Node3D)
                    {
                        Node3D instPrefab3D = (Node3D)instantiatedPrefab;
                        instPrefab3D.Position = new Vector3(x * prefabSize.X, 0, y * prefabSize.Y);

                        if(instPrefab3D is WFCSpawnedInstance3D)
                        {
                            var spawnedInstance3D = (WFCSpawnedInstance3D)instPrefab3D;
                            spawnedInstance3D.ConfigureSpawnedInstance(nodes[nodePosition].tiles[0]);
                        }

                        nodeList.Add(instPrefab3D);
                        this.AddChild(instPrefab3D);
                    }
                    else if(instantiatedPrefab is Node2D)
                    {
                        Node2D instPrefab2D = (Node2D)instantiatedPrefab;
                        instPrefab2D.Position = new Vector2(x * prefabSize.X, y * prefabSize.Y);

                        nodeList.Add(instPrefab2D);
                        this.AddChild(instPrefab2D);
                    }
                }
            }
            outputString += "\n";
        }

        GD.Print(outputString);

    }
}
