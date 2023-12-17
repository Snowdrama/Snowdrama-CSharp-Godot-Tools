using Godot;
using System;
using System.Collections.Generic;


//this is essentuially the same as the 2D prefab generator, but it uses debug images instead of prefabs
//so you can use it to debug the actual raw data faster and make sure you don't have messed up
//data where rooms can't spawn or something.
public partial class WFC2DDebugGenerator : Node
{
    [Export(PropertyHint.File)]
    string tileFilePath;
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
        //generatedNodes = generator.GenerateMap(tileFilePath, mapWidth, mapHeight);
        //OutputDebugTextures(mapWidth, mapHeight, generatedNodes);
        CommandConsole.AddCommand("DetailNode", DetailNode);
        generatedNodes = generator.StartGenerator(tileFilePath, mapWidth, mapHeight);
    }

    public override void _EnterTree()
    {
    }
    public override void _ExitTree()
    {
        CommandConsole.AddCommand("DetailNode", DetailNode);
    }

    bool didCollapse = true;
    public override void _Input(InputEvent @event)
    {

        if (@event.IsActionPressed("F1"))
        {
            generatedNodes = generator.StartGenerator(tileFilePath, mapWidth, mapHeight);
        }

        if (@event.IsActionPressed("F2"))
        {
            generatedNodes = generator.UpdateGenerator(generatedNodes, mapWidth, mapHeight);
        }

        if (@event.IsActionPressed("F3"))
        {
            (generatedNodes, didCollapse) = generator.CollapseNode(generatedNodes, mapWidth, mapHeight);
        }

        if (@event.IsActionPressed("F4"))
        {
            OutputDebugTextures(mapWidth, mapHeight, generatedNodes);
        }

        if (@event.IsActionPressed("F5"))
        {
            generatedNodes = generator.StartGenerator(tileFilePath, mapWidth, mapHeight);
            do
            {
                generatedNodes = generator.UpdateGenerator(generatedNodes, mapWidth, mapHeight);
                (generatedNodes, didCollapse) = generator.CollapseNode(generatedNodes, mapWidth, mapHeight);
            } while (didCollapse);
            OutputDebugTextures(mapWidth, mapHeight, generatedNodes);
        }
    }

    double generateTime = 0.025f;
    double generateTimeMax = 0.025f;
    public override void _Process(double delta)
    {
        base._Process(delta);
        //generateTime -= delta;
        //if (generateTime <= 0)
        //{
        //    generateTime = generateTimeMax;

        //    generatedNodes = generator.UpdateGenerator(generatedNodes, mapWidth, mapHeight);
        //    (generatedNodes, didCollapse) = generator.CollapseNode(generatedNodes, mapWidth, mapHeight);
        //    OutputDebugTextures(mapWidth, mapHeight, generatedNodes);
        //}
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

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var position = new Vector2I(x, y);
                if (nodes.ContainsKey(position))
                {
                    var tempNode = nodes[position];
                    if (tempNode != null && tempNode.entropy == int.MaxValue)
                    {
                        //assume the 0 tile is an empty
                        WFCTile tile = generator.GetTile(0);

                        //if this is 0 then the tile failed to find a pair...
                        if (tempNode.tiles.Count != 0)
                        {
                            tile = tempNode.tiles[0];
                        }
                        var texture = ResourceLoader.Load<Texture2D>(tile.DebugTextureResource);
                        var size = texture.GetSize();
                        var newNode = new Sprite2D()
                        {
                            Texture = ResourceLoader.Load<Texture2D>(tile.DebugTextureResource),
                            Position = tempNode.Position * texture.GetSize(),
                            Rotation = Mathf.DegToRad(tile.Rotation * 90),
                            FlipH = tile.Flip
                        };

                        debugSpriteNodes.Add(newNode);
                        this.AddChild(newNode);
                    }
                }
            }
        }
    }


    public void DetailNode(int x, int y)
    {
        GD.Print(generatedNodes[new Vector2I(x, y)]);
    }
}
