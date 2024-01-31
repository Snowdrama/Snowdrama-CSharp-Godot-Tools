using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[GlobalClass, Tool]
public partial class UIImageGridLayout : Container
{
    [Export] CompressedTexture2D paletteImage;
    Dictionary<Color, int> paletteIndexes = new Dictionary<Color, int>();
    [Export] CompressedTexture2D layoutImage;
    public Dictionary<int, Rect2I> rects = new Dictionary<int, Rect2I>();
    private Vector2 pixelRatio = new Vector2();

    [Export] bool update;
    public override void _Process(double delta)
    {
        if(paletteImage == null || layoutImage == null)
        {
            return;
        }

        if (Engine.IsEditorHint())
        {
            if (update)
            {
                GD.Print("Updating");
                update = false;
                ParsePalette(paletteImage.GetImage());
                ParseLayout(layoutImage.GetImage());
                pixelRatio = new Vector2(1.0f / layoutImage.GetWidth(), 1.0f / layoutImage.GetHeight());
                // Must re-sort the children
                int currentIndex = 1;
                foreach (Control c in GetChildren().Cast<Control>())
                {
                    // Fit to own GRID_SIZE
                    if (rects.ContainsKey(currentIndex))
                    {
                        var currentRect = rects[currentIndex];
                        var anchorEndPixel = currentRect.End + new Vector2I(1, 1);
                        var anchorEnd = anchorEndPixel * pixelRatio;
                        var anchorPositionPixel = currentRect.Position;
                        var anchorPosition = anchorPositionPixel * pixelRatio;

                        c.SetAnchorAndOffset(Side.Left, anchorPosition.X, 0);
                        c.SetAnchorAndOffset(Side.Right, anchorEnd.X, 0);
                        c.SetAnchorAndOffset(Side.Top, anchorPosition.Y, 0);
                        c.SetAnchorAndOffset(Side.Bottom, anchorEnd.Y, 0);
                        currentIndex++;
                    }
                    else
                    {
                        Debug.LogError("Layout doesn't have enough rects to cover all children."
                        + "Please update the layout image or remove children");
                    }
                }
            }
        }
    }
    
    public void ParsePalette(Image paletteData){
        int currentIndex = 0;
        paletteIndexes.Clear();
        for(int y = 0; y < paletteData.GetHeight(); y++)
        {
            for(int x = 0; x < paletteData.GetWidth(); x++)
            {
                var color = paletteData.GetPixel(x, y);
                if(!paletteIndexes.ContainsKey(color)){
                    paletteIndexes.Add(color, currentIndex);
                    currentIndex++;
                }
            }
        }
    }

    public void ParseLayout(Image layoutImageData)
    {
        rects.Clear();
        for(int y = 0; y < layoutImageData.GetHeight(); y++)
        {
            for(int x = 0; x < layoutImageData.GetWidth(); x++)
            {
                var color = layoutImageData.GetPixel(x, y);
                if(!paletteIndexes.ContainsKey(color)){
                    continue;
                }
                var index = paletteIndexes[color];
                if(index == 0){
                    continue;
                }
                if(rects.ContainsKey(index)){
                    //update the rect with new width/height/Offset Data
                    Rect2I rect = rects[index];
                    if(x < rect.Position.X){
                        rect.Position = new Vector2I();
                    }
                    if(x > rect.End.X - 1){
                        rect = rect.GrowSide(Side.Right, Mathf.Abs(x - rect.End.X));
                    }
                    if(y < rect.Position.Y){
                        rect = rect.GrowSide(Side.Top, Mathf.Abs(y - rect.Position.Y));
                    }
                    if(y > rect.End.Y- 1){
                        rect = rect.GrowSide(Side.Bottom, Mathf.Abs(y - rect.End.Y));
                    }
                    rects[index] = rect;
                }
                else{
                    
                    // create a new rectangle
                    Rect2I newRect = new Rect2I
                    {
                        Position = new Vector2I(x, y),
                        Size = new Vector2I(1, 1)
                    };
                    rects.Add(index, newRect);
                }
            }
        }
    }
}