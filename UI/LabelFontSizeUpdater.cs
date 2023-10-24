using Godot;
using System;


[Tool]
public partial class LabelFontSizeUpdater : Label
{
    public override void _Ready()
    {
        parent = GetParent<Control>();
    }

    private Vector2 oldParentSize;
    private int oldTextCharCount;


    private Control parent;
    private Font font;
    private int fontSize;

    public override void _Process(double delta)
    {
        parent ??= this.GetParent<Control>();
        font ??= this.GetThemeFont("font");
        if (oldTextCharCount != Text.Length || oldParentSize != parent.Size)
        {
            this.Set("theme_override_font_sizes/font_size", 1);
            UpdateFontSize();
            oldParentSize = parent.Size;
            oldTextCharCount = Text.Length;
        }
    }

    public void OnResize()
    {
        UpdateFontSize();
    }


    public void UpdateFontSize()
    {
        fontSize = CalculateFontSize(parent.Size, font);
        this.Set("theme_override_font_sizes/font_size", fontSize);
    }
    public int CalculateFontSize(Vector2 localMaxSize, Font localFont)
    {
        //GD.Print($"_____________________________________________________________________________");
        //we size it down just to make sure we get an accurate max size for the parent size.
        this.Set("theme_override_font_sizes/font_size", 1);
        //GD.Print($"maxSize: {localMaxSize}");
        var fontSize = 1;
        Vector2 size = localFont.GetMultilineStringSize(text: this.Text, width: localMaxSize.X, fontSize: fontSize);
        //GD.Print($"Size at {fontSize}: {size}");
        for (int i = 0; i < 3; i++)
        {
            fontSize = fontSize.Clamp(1, 100_000);
            size = localFont.GetMultilineStringSize(text: this.Text, width: localMaxSize.X, fontSize: fontSize);
            //GD.Print($"Size at {fontSize}: {size}");
            
            float testRatioX = Mathf.FloorToInt(localMaxSize.X / size.X);
            
            //GD.Print($"The ratio of the: {localMaxSize.X} / {size.X} = {testRatioX}");
            float testRatioY = Mathf.FloorToInt(localMaxSize.Y / size.Y);
            //GD.Print($"The ratio of the: {localMaxSize.Y} / {size.Y} = {testRatioY}");
            
            Vector2 targetSizeUsingXRatio = size * testRatioX;
            Vector2 targetSizeUsingYRatio = size * testRatioY;
            
            if(size.X > size.Y)
            {
                //GD.Print($"Since the line of text is wider than tall, we should fit to the X axis");
                //GD.Print($"Using the {testRatioX} by the X Size {size.X} will make it {size.X * testRatioX}");
                //GD.Print($"Which should fit inside {localMaxSize.X} pixels wide");

                //GD.Print($"Max Size: {localMaxSize}");
                //GD.Print($"Test new Size X Ratio {targetSizeUsingXRatio}");
                //GD.Print($"Test new Size Y Ratio {targetSizeUsingYRatio}");
                if (targetSizeUsingXRatio.X <= localMaxSize.X && targetSizeUsingXRatio.Y <= localMaxSize.Y)
                {
                    //this size is okay we can use it
                    fontSize = Mathf.FloorToInt(fontSize * testRatioX);
                }
                else if(targetSizeUsingYRatio.X <= localMaxSize.X && targetSizeUsingYRatio.Y <= localMaxSize.Y){

                    //this size is okay we can use it
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
                //GD.Print($"Using the {testRatioY} by the Y Size {size.Y} will make it {size.Y * testRatioY}");
                //GD.Print($"will make the text fit in the {localMaxSize.Y} pixels wide");

                //GD.Print($"Max Size: {localMaxSize}");
                //GD.Print($"Test new Size X Ratio {targetSizeUsingXRatio}");
                //GD.Print($"Test new Size Y Ratio {targetSizeUsingYRatio}");

                if(targetSizeUsingYRatio.X <= localMaxSize.X && targetSizeUsingYRatio.Y <= localMaxSize.Y)
                {
                    //this size is okay we can use it
                    fontSize = Mathf.FloorToInt(fontSize * testRatioY);
                }
                else if(targetSizeUsingXRatio.X <= localMaxSize.X && targetSizeUsingXRatio.Y <= localMaxSize.Y)
                {
                    //this size is okay we can use it
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
