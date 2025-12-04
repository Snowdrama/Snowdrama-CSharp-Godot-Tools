using Godot;

[Tool, GlobalClass]
public partial class TileColliderFromImage : TileMapLayer
{
    [Export] Texture2D? sourceTexture;
    TileSet? tileSet;
    [ExportToolButton("Create Collider!")]
    public Callable DrawTilesButton => Callable.From(CreateCollider);

    public void CreateCollider()
    {
        tileSet = this.TileSet;

        if (sourceTexture == null)
        {
            Debug.LogError($"Source Texture needs to be set!");
            return;
        }

        var sourceImage = sourceTexture.GetImage();
        for (int y = 0; y < sourceImage.GetSize().Y; y++)
        {
            for (int x = 0; x < sourceImage.GetSize().X; x++)
            {
                var color = sourceImage.GetPixel(x, y);
                if (color.A <= 0) { continue; }

                var pos = new Vector2I(x, y);
                this.SetCell(pos, tileSet.GetSourceId(0), Vector2I.Zero);
            }
        }
    }
}
