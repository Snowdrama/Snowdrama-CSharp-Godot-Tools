using Godot;
using System;

public partial class VideoOptionsManager : Node
{
	public override void _Ready()
    {
        WindowManager.WindowedResolution = GameWindowResolutions.resolutions[(int)Options.GetInt(Options.WINDOWED_RESOLUTION_OPTION_KEY)];
        Debug.Log($"Loading Graphics Settings, Window Resolution:{WindowManager.WindowedResolution}");

        WindowManager.VSyncMode = (DisplayServer.VSyncMode)Options.GetInt(Options.VSYNC_OPTION_KEY, 0);
        Debug.Log($"Loading Graphics Settings, VSync Mode:{WindowManager.VSyncMode}");

        WindowManager.CurrentWindowMode = (DisplayServer.WindowMode)Options.GetInt(Options.WINDOW_MODE_OPTION_KEY, 1);
        Debug.Log($"Loading Graphics Settings, Window Mode:{WindowManager.CurrentWindowMode}");
    }
}
