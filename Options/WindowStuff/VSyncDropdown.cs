using Godot;
using Godot.Collections;
using System;

public partial class VSyncDropdown : OptionButton
{

    [Export]
    public Array<string> Values = new Array<string>
    {
        "GRAPHICS_VSYNC_OFF",
        "GRAPHICS_VSYNC_ON",
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
        if(index >= 0 && index <= 3)
        {
            WindowManager.VSyncMode = (DisplayServer.VSyncMode)index;
            Options.SetInt(Options.VSYNC_OPTION_KEY, (int)index);
        }
        else
        {
            Debug.LogError("VSync index must be 0-3, Disabled, Enabled, Adaptive, Mailbox");
        }
    }

    private void LoadOptionFromSave()
    {
        var vsync = Options.GetInt(Options.VSYNC_OPTION_KEY, (int)DisplayServer.VSyncMode.Enabled);
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
