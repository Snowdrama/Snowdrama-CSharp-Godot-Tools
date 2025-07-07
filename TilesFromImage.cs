using Godot;
using Godot.Collections;

[GlobalClass, Tool]
public partial class TilesFromImage : TileMapLayer
{
    [Export] Texture2D atlasTemplate;
    [Export] Texture2D sourceTexture;
    TileSet tileSet;

    [ExportToolButton("Load Atlas")]
    public Callable LoadAtlasTemplateButton => Callable.From(LoadAtlasTemplate);
    [ExportToolButton("Draw Tiles!")]
    public Callable DrawTilesButton => Callable.From(DrawTiles);

    [Export]
    Dictionary<Color, Vector2I> colorIndexes = new Dictionary<Color, Vector2I>();


    public void LoadAtlasTemplate()
    {
        Image atlasImage = atlasTemplate.GetImage();
        for (int y = 0; y < atlasTemplate.GetSize().Y; y++)
        {
            for (int x = 0; x < atlasTemplate.GetSize().X; x++)
            {
                var color = atlasImage.GetPixel(x, y);
                var pos = new Vector2I(x, y);

                if (!colorIndexes.ContainsKey(color))
                {
                    colorIndexes.Add(color, pos);
                }
            }
        }
    }


    public void DrawTiles()
    {
        Debug.Log($"Hello World!");
        tileSet = this.TileSet;


        Image sourceImage = sourceTexture.GetImage();
        for (int y = 0; y < sourceTexture.GetSize().Y; y++)
        {
            for (int x = 0; x < sourceTexture.GetSize().X; x++)
            {
                var color = sourceImage.GetPixel(x, y);
                if (colorIndexes.ContainsKey(color))
                {
                    var colorIndexPos = colorIndexes[color];
                    var pos = new Vector2I(x, y);

                    this.SetCell(new Vector2I(x, y), tileSet.GetSourceId(0), colorIndexPos);
                }
            }
        }

    }
}
