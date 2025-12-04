using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Send a message to the map blitter that reveals a section
/// 
/// Takes a position and a string that is the key that corresponsds with the shape to blit
/// </summary>
public class BlitRevealToMapMessage : AMessage<Vector2I, string> { }

public struct MapBlitData
{
    public string key;
    public Vector2 position;
    public Rect2I sourceRect;
    public Vector2I destPosition;

}
public partial class DrawPixelsWithSprite : Sprite2D
{
    [Export(PropertyHint.Range, "0.0, 1.0, 0.001")] private float pixelShowSpeed = 1.0f;
    [Export] private Vector2I imageSize = new Vector2I(2048, 2048);
    private ImageTexture drawTexture;
    private Image image;

    private Texture2D myTexture;
    private Image myTextureImage;


    private Vector2I pixelPos;
    private int posX;
    private int posY;

    [Export] private Godot.Collections.Dictionary<string, Texture2D> mapBlitTextures = [];
    private Godot.Collections.Dictionary<string, Image> mapBlitImages = [];


    private List<string> queuedNames = [];
    private Queue<MapBlitData> queueToBlit = new();
    public override void _Ready()
    {
        base._Ready();
        foreach (var item in mapBlitTextures)
        {
            mapBlitImages.Add(item.Key, item.Value.GetImage());
        }

        drawTexture = ImageTexture.CreateFromImage(Image.CreateEmpty(imageSize.X, imageSize.Y, false, Image.Format.Rgbaf));
        image = drawTexture.GetImage();

        image.Fill(new Color(1, 1, 1, 0.0f));
        drawTexture.Update(image);

        this.Texture = drawTexture;

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        //todo, maybe we should only blit once per update?
        foreach (var mapBlitData in queueToBlit)
        {
            Vector2 imageRelativePos = (
                    mapBlitData.destPosition +
                    this.GlobalPosition.RoundToInt() +
                    ((Vector2)imageSize * this.GlobalScale * 0.5f) -
                    ((Vector2)mapBlitData.sourceRect.Size * this.GlobalScale * 0.5f)
                ) / this.GlobalScale;

            var textureToBlit = mapBlitImages[mapBlitData.key];

            var showSpeedThisTick = pixelShowSpeed * (float)delta;

            for (int y = 0; y < textureToBlit.GetHeight(); y++)
            {
                for (int x = 0; x < textureToBlit.GetWidth(); x++)
                {
                    var pixelCoord = imageRelativePos.RoundToInt() + new Vector2I(x, y);
                    //only try and draw the pixel if we're in bounds obviously
                    if (!pixelCoord.InBounds(Vector2.Zero, image.GetSize()))
                    {
                        continue;
                    }

                    var sourcePixel = textureToBlit.GetPixel(x, y);
                    var targetPixel = image.GetPixelv(pixelCoord);
                    if (sourcePixel.A > targetPixel.A)
                    {
                        var nextAlpha = Mathf.MoveToward(targetPixel.A, sourcePixel.A, showSpeedThisTick);

                        //if (x == 8 && y == 8)
                        //{
                        //    Debug.Cyan($"Old Pixel: {targetPixel}");
                        //    Debug.Cyan($"Trying to set: {sourcePixel}");
                        //    Debug.Yellow($"Mathf.MoveToward({targetPixel.A}, {sourcePixel.A}, {showSpeedThisTick}) -> {nextAlpha}");
                        //}

                        sourcePixel.A = nextAlpha;

                        //if (x == 8 && y == 8)
                        //{
                        //    Debug.Green($"Setting New Pixel: {sourcePixel}");
                        //    Debug.Log($"-------------------------------------------");
                        //}
                        image.SetPixelv(pixelCoord, new Color(sourcePixel.R, sourcePixel.G, sourcePixel.B, nextAlpha));
                    }
                }
            }
        }
        queueToBlit.Clear();

        drawTexture.Update(image);
    }

    private BlitRevealToMapMessage BlitRevealToMapMessage;
    public override void _EnterTree()
    {
        base._EnterTree();
        BlitRevealToMapMessage = Messages.Get<BlitRevealToMapMessage>();
        BlitRevealToMapMessage.AddListener(BlitRevealToMap);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        BlitRevealToMapMessage.RemoveListener(BlitRevealToMap);
        BlitRevealToMapMessage = null;
        Messages.Return<BlitRevealToMapMessage>();
    }

    public void BlitRevealToMap(Vector2I position, string blitKey)
    {
        if (mapBlitImages.ContainsKey(blitKey))
        {
            var image = mapBlitImages[blitKey];
            queueToBlit.Enqueue(new MapBlitData()
            {
                key = blitKey,
                sourceRect = new Rect2I(Vector2I.Zero, image.GetSize()),
                destPosition = position
            });
        }
    }
}
