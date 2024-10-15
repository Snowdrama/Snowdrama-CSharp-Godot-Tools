using Godot;
using Godot.Collections;
using System;

public partial class VSyncDropdown : OptionButton
{

    [Export]
    public Array<string> Values = new Array<string>
    {
        "GRAPHICS_VSYNC_ON",
        "GRAPHICS_VSYNC_OFF",
        "GRAPHICS_VSYNC_ADAPTIVE",
        "GRAPHICS_VSYNC_MAILBOX",
    };
    public override void _Ready()
    {
        this.ItemSelected += VSyncToggle_ItemSelected;
        this.VisibilityChanged += OnVisibilityChanged;
        LoadOptions();
        LoadOptionFromSave();
    }

    private void OnVisibilityChanged()
    {
        LoadOptions();
        LoadOptionFromSave();
    }

    private void VSyncToggle_ItemSelected(long index)
    {
        switch (index)
        {
            case 0:
                WindowManager.VSyncMode = DisplayServer.VSyncMode.Enabled;
                break;
            case 1:
                WindowManager.VSyncMode = DisplayServer.VSyncMode.Disabled;
                break;
            case 2:
                WindowManager.VSyncMode = DisplayServer.VSyncMode.Adaptive;
                break;
            case 3:
                WindowManager.VSyncMode = DisplayServer.VSyncMode.Mailbox;
                break;
        }
        Options.SetInt(Options.VSYNC_OPTION_KEY, (int)index);
    }

    private void LoadOptionFromSave()
    {
        var vsync = Options.GetInt(Options.VSYNC_OPTION_KEY, 0);
        this.Select(vsync);
    }

    private void LoadOptions()
    {
        this.Clear();
        for (int i = 0; i < Values.Count; i++)
        {
            this.AddItem($"{Values[i]}");
        }

    }
}
