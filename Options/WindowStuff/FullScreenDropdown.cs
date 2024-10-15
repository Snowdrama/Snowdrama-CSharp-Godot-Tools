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
        this.AddItem("GRAPHICS_SCREEN_MODE_FULLSCREEN", 1);
        UpdateOptionToValue();
    }

    private void OnItemSelected(long selected)
    {
        switch (selected)
        {
            case 0:
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Windowed;
                break;
            case 1:
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Fullscreen;
                break;
            default:
                this.Select(0);
                WindowManager.CurrentWindowMode = DisplayServer.WindowMode.Windowed;
                break;
        }
    }

    public void UpdateOptionToValue()
    {
        var selection = Options.GetInt(Options.DISPLAY_MODE_OPTION_KEY, 0);
        switch ((DisplayServer.WindowMode)selection)
        {
            case DisplayServer.WindowMode.Windowed:
                this.Select(0);
                break;
            case DisplayServer.WindowMode.Fullscreen:
                this.Select(1);
                break;
            default:
                this.Select(0);
                break;
        }
    }
}
