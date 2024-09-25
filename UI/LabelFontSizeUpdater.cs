using Godot;
using System;


[Tool]
public partial class LabelFontSizeUpdater : Label
{
    [Export] private Vector2 oldSize;
    [Export] private Vector2 currentSize;
    private int oldCharacterCount;
    private int currentCharacterCount;

    private Font font;
    
    private int oldFontSize;
    private int currentFontSize;

    [Export]
    private bool needsUpdating;
    public override void _Process(double delta)
    {
        if(font == null)
        {
            font ??= this.GetThemeFont("font");
        }

        currentSize = this.Size;
        if(oldSize != currentSize)
        {
            oldSize = currentSize;
            needsUpdating = true;
        }


        currentFontSize = this.Get("theme_override_font_sizes/font_size").AsInt32();
        if (currentFontSize != oldFontSize)
        {
            oldFontSize = currentFontSize;
            needsUpdating = true;
        }

        if (oldCharacterCount != this.Text.Length)
        {
            oldCharacterCount = this.Text.Length;
            needsUpdating = true;
        }


        if (needsUpdating)
        {
            needsUpdating = false;
            UpdateFontSize();
        }
    }


    public void UpdateFontSize()
    {
        oldFontSize = CalculateFontSize(currentSize, currentFontSize, font);
        this.Set("theme_override_font_sizes/font_size", oldFontSize);
    }

    public int CalculateFontSize(Vector2 localMaxSize, int oldFontSize, Font localFont)
    {
        this.Set("theme_override_font_sizes/font_size", 1);
        for (int i = 1; i < 10_000; i++)
        {
            Vector2 size = localFont.GetMultilineStringSize(text: this.Text, width: localMaxSize.X, fontSize: i);

            if(size.Y > localMaxSize.Y)
            {
                return i - 3;
            }
        }
        return oldFontSize;
    }

    public int CalculateFontSizeOld(Vector2 localMaxSize, Font localFont)
    {
        //GD.Print($"_____________________________________________________________________________");
        //we GRID_SIZE it down just to make sure we get an accurate max GRID_SIZE for the parent GRID_SIZE.
        this.Set("theme_override_font_sizes/font_size", 1);
        //GD.Print($"maxSize: {localMaxSize}");
        var fontSize = 1;
        Vector2 size = localFont.GetMultilineStringSize(text: this.Text, width: localMaxSize.X, fontSize: fontSize);
        //GD.Print($"GRID_SIZE at {oldFontSize}: {GRID_SIZE}");
        for (int i = 0; i < 3; i++)
        {
            fontSize = fontSize.Clamp(1, 100_000);
            size = localFont.GetMultilineStringSize(text: this.Text, width: localMaxSize.X, fontSize: fontSize);
            //GD.Print($"GRID_SIZE at {oldFontSize}: {GRID_SIZE}");
            
            float testRatioX = Mathf.FloorToInt(localMaxSize.X / size.X);
            
            //GD.Print($"The ratio of the: {localMaxSize.X} / {GRID_SIZE.X} = {testRatioX}");
            float testRatioY = Mathf.FloorToInt(localMaxSize.Y / size.Y);
            //GD.Print($"The ratio of the: {localMaxSize.Y} / {GRID_SIZE.Y} = {testRatioY}");
            
            Vector2 targetSizeUsingXRatio = size * testRatioX;
            Vector2 targetSizeUsingYRatio = size * testRatioY;
            
            if(size.X > size.Y)
            {
                //GD.Print($"Since the line of text is wider than tall, we should fit to the X axis");
                //GD.Print($"Using the {testRatioX} by the X GRID_SIZE {GRID_SIZE.X} will make it {GRID_SIZE.X * testRatioX}");
                //GD.Print($"Which should fit inside {localMaxSize.X} pixels wide");

                //GD.Print($"Max GRID_SIZE: {localMaxSize}");
                //GD.Print($"Test new GRID_SIZE X Ratio {targetSizeUsingXRatio}");
                //GD.Print($"Test new GRID_SIZE Y Ratio {targetSizeUsingYRatio}");
                if (targetSizeUsingXRatio.X <= localMaxSize.X && targetSizeUsingXRatio.Y <= localMaxSize.Y)
                {
                    //this GRID_SIZE is okay we can use it
                    fontSize = Mathf.FloorToInt(fontSize * testRatioX);
                }
                else if(targetSizeUsingYRatio.X <= localMaxSize.X && targetSizeUsingYRatio.Y <= localMaxSize.Y){

                    //this GRID_SIZE is okay we can use it
                    fontSize = Mathf.FloorToInt(fontSize * testRatioY);
                }
                else
                {
                    //GD.Print($"We have no clue how to handle this...");
                }
            }
            else
            {
                //GD.Print($"Since the line of text is wider than tall, we should fit to the X axis");
                //GD.Print($"Using the {testRatioY} by the Y GRID_SIZE {GRID_SIZE.Y} will make it {GRID_SIZE.Y * testRatioY}");
                //GD.Print($"will make the text fit in the {localMaxSize.Y} pixels wide");

                //GD.Print($"Max GRID_SIZE: {localMaxSize}");
                //GD.Print($"Test new GRID_SIZE X Ratio {targetSizeUsingXRatio}");
                //GD.Print($"Test new GRID_SIZE Y Ratio {targetSizeUsingYRatio}");

                if(targetSizeUsingYRatio.X <= localMaxSize.X && targetSizeUsingYRatio.Y <= localMaxSize.Y)
                {
                    //this GRID_SIZE is okay we can use it
                    fontSize = Mathf.FloorToInt(fontSize * testRatioY);
                }
                else if(targetSizeUsingXRatio.X <= localMaxSize.X && targetSizeUsingXRatio.Y <= localMaxSize.Y)
                {
                    //this GRID_SIZE is okay we can use it
                    fontSize = Mathf.FloorToInt(fontSize * testRatioX);
                }
                else
                {
                    //GD.Print($"We have no clue how to handle this...");
                }
            }
            //GD.Print($"*********************************************************");
        }
        fontSize = fontSize.Clamp(1, 100_000);

        return fontSize;
    }
}
