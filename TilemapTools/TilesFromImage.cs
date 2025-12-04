using Godot;
using Godot.Collections;

[GlobalClass, Tool]
public partial class TilesFromImage : TileMapLayer
{
    [Export] Texture2D? colorsTemplate;
    [Export] Texture2D? sourceTexture;
    TileSet? tileSet;

    [Export] int tileSetIndex = 0;
    [ExportToolButton("Load Atlas")]
    public Callable LoadAtlasTemplateButton => Callable.From(LoadAtlasTemplate);
    [ExportToolButton("Draw Tiles!")]
    public Callable DrawTilesButton => Callable.From(DrawTiles);

    [Export]
    Dictionary<Color, Vector2I> colorIndexes = new Dictionary<Color, Vector2I>();

    [ExportCategory("Auto Tile")]
    [Export] bool isAutoTileLayer;
    [Export] int terrainSetLayer = 0;
    [Export] int terrainLayer = 3;

    public void LoadAtlasTemplate()
    {
        if (colorsTemplate == null)
        {
            Debug.LogError("Atlas Template is Null!");
            return;
        }
        Image atlasImage = colorsTemplate.GetImage();
        for (int y = 0; y < colorsTemplate.GetSize().Y; y++)
        {
            for (int x = 0; x < colorsTemplate.GetSize().X; x++)
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
        tileSet = this.TileSet;

        if (sourceTexture == null)
        {
            Debug.LogError("SourceTexture is Null!");
            return;
        }
        if (tileSet == null)
        {
            Debug.LogError("TileSet is Null!");
            return;
        }
        Image sourceImage = sourceTexture.GetImage();

        //auto tiles need to be handled differently because we need to process/connect them later.
        if (isAutoTileLayer)
        {
            ProcessAutoTiles(sourceImage, tileSet);
        }
        else
        {
            ProcessNormalTiles(sourceImage, tileSet);
        }
    }

    public void ProcessAutoTiles(Image sourceImage, TileSet set)
    {
        Array<Vector2I> terrainCells = new Array<Vector2I>();
        for (int y = 0; y < sourceImage.GetSize().Y; y++)
        {
            for (int x = 0; x < sourceImage.GetSize().X; x++)
            {
                var color = sourceImage.GetPixel(x, y);
                if (color.A <= 0)
                {
                    var pos = new Vector2I(x, y);
                    this.SetCell(pos, -1, null, 0);
                }
                else if (colorIndexes.ContainsKey(color))
                {
                    var colorIndexPos = colorIndexes[color];
                    var pos = new Vector2I(x, y);
                    this.SetCell(pos, set.GetSourceId(tileSetIndex), colorIndexPos);
                    terrainCells.Add(pos);
                }
            }
        }
        this.SetCellsTerrainConnect(terrainCells, terrainSetLayer, terrainLayer);
    }

    public void ProcessNormalTiles(Image sourceImage, TileSet set)
    {
        for (int y = 0; y < sourceImage.GetSize().Y; y++)
        {
            for (int x = 0; x < sourceImage.GetSize().X; x++)
            {
                var color = sourceImage.GetPixel(x, y);
                if (color.A <= 0) { continue; }
                if (colorIndexes.ContainsKey(color))
                {
                    var colorIndexPos = colorIndexes[color];
                    var pos = new Vector2I(x, y);
                    this.SetCell(pos, set.GetSourceId(tileSetIndex), colorIndexPos);
                }
            }
        }
    }
}
