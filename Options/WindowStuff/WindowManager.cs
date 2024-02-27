using Godot;
using System;

//this essentially does everything window management, including saving settings since multiple things
//can change the settings

//This also sends messages when things are changed!
public partial class WindowManager : Node
{
    private static Vector2I _windowedResolution = Options.DEFAULT_WINDOW_RESOLUTION;
    public static Vector2I WindowedResolution
    {
        get
        {
            return _windowedResolution;
        }
        set
        {
            _windowedResolution = value;
            SetWindowResolution(_windowedResolution);
        }
    }
    private static DisplayServer.WindowMode _currentWindowMode = DisplayServer.WindowMode.Windowed;
    public static DisplayServer.WindowMode CurrentWindowMode
    {
        get 
        { 
            return _currentWindowMode;
        }
        set 
        { 
            _currentWindowMode = value;
            WindowSetMode((DisplayServer.WindowMode)_currentWindowMode);
        }
    }

    private static DisplayServer.VSyncMode _vSyncMode = DisplayServer.VSyncMode.Enabled;

    public static DisplayServer.VSyncMode VSyncMode
    {
        get 
        { 
            return _vSyncMode;
        }
        set
        {
            _vSyncMode = value;
            DisplayServer.WindowSetVsyncMode(_vSyncMode);
        }
    }

    public override void _Ready()
    {
        WindowedResolution = Options.GetVector2I(Options.WINDOWED_RESOLUTION_OPTION_KEY, Options.DEFAULT_WINDOW_RESOLUTION);
        CurrentWindowMode = (DisplayServer.WindowMode)Options.GetInt(Options.DISPLAY_MODE_OPTION_KEY, (int)DisplayServer.WindowMode.Windowed);
        VSyncMode = (DisplayServer.VSyncMode)Options.GetInt(Options.VSYNC_OPTION_KEY, (int)DisplayServer.VSyncMode.Enabled);
    }

    private static void SetWindowResolution(Vector2I windowResolution)
    {
        if(CurrentWindowMode != DisplayServer.WindowMode.Windowed)
        {
            WindowSetMode(DisplayServer.WindowMode.Windowed);
        }
        DisplayServer.WindowSetSize(windowResolution);
        Options.SetVector2I(Options.WINDOWED_RESOLUTION_OPTION_KEY, windowResolution);
        Messages.GetOnce<ScreenResolutionChangedMessage>().Dispatch(windowResolution);
    }

    private static void WindowSetMode(DisplayServer.WindowMode mode)
    {
        var oldWindowMode = DisplayServer.WindowGetMode();
        switch (mode)
        {
            case DisplayServer.WindowMode.Windowed:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                SetWindowResolution(WindowedResolution);
                Messages.GetOnce<ScreenModeChangedMessage>().Dispatch(DisplayServer.WindowMode.Windowed);
                break;
            case DisplayServer.WindowMode.Minimized:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Minimized);
                Messages.GetOnce<ScreenModeChangedMessage>().Dispatch(DisplayServer.WindowMode.Minimized);
                break;
            case DisplayServer.WindowMode.Maximized:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
                Messages.GetOnce<ScreenModeChangedMessage>().Dispatch(DisplayServer.WindowMode.Maximized);
                break;
            case DisplayServer.WindowMode.Fullscreen:
                DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                Messages.GetOnce<ScreenModeChangedMessage>().Dispatch(DisplayServer.WindowMode.Fullscreen);
                break;
            //case DisplayServer.WindowMode.ExclusiveFullscreen:
            //    DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
            //    SetWindowResolution(DisplayServer.ScreenGetSize());
            //    break;
        }
        Options.SetInt(Options.DISPLAY_MODE_OPTION_KEY, (int)mode);
    }
}

