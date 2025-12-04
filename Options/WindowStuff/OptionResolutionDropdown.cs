using Godot;
using System;
using Godot.Collections;

//This class just handles the changing of the resolution with the WindowManager

//The WindowManager must be added as a auto-load for this to work

public static class GameWindowResolutions
{
    public static Array<Vector2I> resolutions = new Array<Vector2I>
    {
        new Vector2I(1280, 720),
        new Vector2I(640, 360),
        new Vector2I(896, 504),
        new Vector2I(960, 540),
        new Vector2I(1024, 576),
        new Vector2I(1200, 600),
        new Vector2I(1280, 720),
        new Vector2I(1366, 768),
        new Vector2I(1600, 900),
        new Vector2I(1920, 1080),
        new Vector2I(2560, 1440),
        new Vector2I(3200, 1800),
        new Vector2I(3840, 2160),
    };
}


[GlobalClass]
public partial class OptionResolutionDropdown : OptionButton
{
	[Export] Vector2I defaultResolution;

    public override void _Ready()
    {
        LoadDataIntoOptionButton();
        this.ItemSelected += OnItemSelected;
        this.VisibilityChanged += OnVisibilityChanged;
        LoadValueFromOptions();
    }

    private void OnItemSelected(long index)
    {
        WindowManager.WindowedResolution = GameWindowResolutions.resolutions[(int)index];
    }

    private void OnVisibilityChanged()
    {
        LoadValueFromOptions();
    }

    private void LoadValueFromOptions()
    {
        var loadedValue = Options.GetVector2I(Options.WINDOWED_RESOLUTION_OPTION_KEY, defaultResolution);
        for (int i = 0; i < GameWindowResolutions.resolutions.Count; i++)
        {
            if (GameWindowResolutions.resolutions[i] == loadedValue)
            {
                this.Select(i);
                return;
            }
        }
        this.Select(0);
    }


    private void LoadDataIntoOptionButton()
    {
        this.Clear();
        for (int i = 0; i < GameWindowResolutions.resolutions.Count; i++)
        {
            if (i == 0)
            {
                this.AddItem($"{GameWindowResolutions.resolutions[i].X} posX {GameWindowResolutions.resolutions[i].Y} (Default)");
            }
            else
            {
                this.AddItem($"{GameWindowResolutions.resolutions[i].X} posX {GameWindowResolutions.resolutions[i].Y}");
            }
        }
    }

    ScreenResolutionChangedMessage ScreenResolutionChangedMessage { get; set; }
    ScreenModeChangedMessage ScreenModeChangedMessage { get; set; }
    public override void _EnterTree()
    {
        ScreenModeChangedMessage = Messages.Get<ScreenModeChangedMessage>();
        ScreenModeChangedMessage.AddListener(ScreenModeChanged);
    }
    public override void _ExitTree()
    {
        ScreenModeChangedMessage.RemoveListener(ScreenModeChanged);
        Messages.Return<ScreenModeChangedMessage>();
    }

    public void ScreenModeChanged(DisplayServer.WindowMode modeChaned)
    {
        if (modeChaned == DisplayServer.WindowMode.Fullscreen || modeChaned == DisplayServer.WindowMode.Maximized)
        {
            this.Disabled = true;
        }
        else
        {
            this.Disabled = false;
        }
    }
}
