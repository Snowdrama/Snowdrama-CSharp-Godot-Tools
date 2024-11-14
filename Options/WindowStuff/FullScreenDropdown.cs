using Godot;
using System;

//This class just handles the changing of the window mode with the WindowManager

//The WindowManager must be added as a auto-load for this to work
[GlobalClass]
public partial class FullScreenDropdown : OptionButton
{
	public override void _Ready()
	{
        this.ItemSelected += OnItemSelected;
        this.Clear();
        this.AddItem("GRAPHICS_SCREEN_MODE_WINDOWED", 0);
        this.AddItem("GRAPHICS_SCREEN_MODE_MAXIMIZED", 2);
        this.AddItem("GRAPHICS_SCREEN_MODE_FULLSCREEN", 3);
        this.AddItem("GRAPHICS_SCREEN_MODE_EXCLUSIVE_FULLSCREEN", 4);
        UpdateOptionToValue();
    }

    private void OnItemSelected(long selected)
    {
        Debug.Log($"Selecting index {selected} which is id: {this.GetItemId((int)selected)} or {(DisplayServer.WindowMode)this.GetItemId((int)selected)}");
        switch (this.GetItemId((int)selected))
        {
            case 0:
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Windowed;
                break;
            case 2:
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Maximized;
                break;
            case 3:
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Fullscreen;
                break;
            case 4:
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
                break;
            default:
                //force windowed if this isnt one of the 4 options
                selected = 0;
                this.Select(0);
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Windowed;
                break;
        }
        Options.SetInt(Options.WINDOW_MODE_OPTION_KEY, (int)selected);
    }

    public void UpdateOptionToValue()
    {
        var selection = Options.GetInt(Options.DISPLAY_MODE_OPTION_KEY, 0);
        switch ((DisplayServer.WindowMode)this.GetItemId((int)selection))
        {
            case DisplayServer.WindowMode.Windowed:
                this.Select(0);
                break;
            case DisplayServer.WindowMode.Maximized:
                this.Select(1);
                break;
            case DisplayServer.WindowMode.Fullscreen:
                this.Select(2);
                break;
            case DisplayServer.WindowMode.ExclusiveFullscreen:
                this.Select(3);
                break;
            default:
                this.Select(0);
                break;
        }
    }
}
