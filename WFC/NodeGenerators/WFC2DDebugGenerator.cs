using Godot;
using System;
using System.Collections.Generic;


//this is essentuially the same as the 2D prefab generator, but it uses debug images instead of prefabs
//so you can use it to debug the actual raw data faster and make sure you don't have messed up
//data where rooms can't spawn or something.
public partial class WFC2DDebugGenerator : Node
{
	[Export]
	WFCDungeonGenerator generator;

    [Export]
    int mapWidth = 32;
    [Export]
    int mapHeight = 32;

    Dictionary<Vector2I, WFCNode> generatedNodes;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        generatedNodes = generator.GenerateMap(mapWidth, mapHeight);
        OutputDebugTextures(mapWidth, mapHeight, generatedNodes);
    }

    List<Sprite2D> debugSpriteNodes = new List<Sprite2D>();
    int debugTileW = 32;
    int debugTileH = 32;
    public void OutputDebugTextures(int mapWidth, int mapHeight, Dictionary<Vector2I, WFCNode> nodes)
    {
        foreach (var item in debugSpriteNodes)
        {
            item.QueueFree();
        }
        debugSpriteNodes.Clear();

        string outputString = "";
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var tempNode = nodes[new Vector2I(x, y)];
                if (tempNode != null && tempNode.entropy == int.MaxValue)
                {
                    var tile = tempNode.tiles[0];
                    var texture = ResourceLoader.Load<Texture2D>(tile.DebugTextureResource);
                    var size = texture.GetSize();
                    var newNode = new Sprite2D()
                    {
                        Texture = ResourceLoader.Load<Texture2D>(tile.DebugTextureResource),
                        Position = tempNode.Position * texture.GetSize(),
                    };

                    debugSpriteNodes.Add(newNode);
                    this.AddChild(newNode);
                }
            }
            outputString += "\n";
        }

        GD.Print(outputString);

    }

}
