using Godot;
using System;

public partial class VideoOptionsManager : Node
{
	public override void _Ready()
    {
        //we only want to try doing this if we're not in the editor... or if we're embedded
        if (!Engine.IsEditorHint() && !Engine.IsEmbeddedInEditor())
        {
            WindowManager.WindowedResolution = GameWindowResolutions.resolutions[(int)Options.GetInt(Options.WINDOWED_RESOLUTION_OPTION_KEY)];
            Debug.Log($"Loading Graphics Settings, Window Resolution:{WindowManager.WindowedResolution}");

            WindowManager.VSyncMode = (DisplayServer.VSyncMode)Options.GetInt(Options.VSYNC_OPTION_KEY, (int)DisplayServer.VSyncMode.Enabled);
            Debug.Log($"Loading Graphics Settings, VSync Mode:{WindowManager.VSyncMode}");

            var loadedMode = (DisplayServer.WindowMode)Options.GetInt(Options.WINDOW_MODE_OPTION_KEY, (int)DisplayServer.WindowMode.Windowed);

            //the game shouldn't launch minimized...
            if (loadedMode == DisplayServer.WindowMode.Minimized)
            {
                loadedMode = DisplayServer.WindowMode.Windowed;
                Options.SetInt(Options.WINDOW_MODE_OPTION_KEY, (int)WindowManager.CurrentWindowMode);
            }
            WindowManager.CurrentWindowMode = loadedMode;
            Debug.Log($"Loading Graphics Settings, Window Mode:{WindowManager.CurrentWindowMode}");
        }
    }
}
