using Godot;
#if IMGUI
using ImGuiNET;
#endif

public partial class InputSchemeSwitcher : Node
{
    //delay is used to prevent erronious button presses during boot like phantom trigger pulls
    double delay = 1.0;
    bool ready;

    [Export] bool mouseMovementSwitchesScheme = true;

    public override void _EnterTree()
    {
        this.ProcessMode = ProcessModeEnum.Always;
        this.SetProcessInput(true);
    }

    public override void _Ready()
    {
        base._Ready();


        if (OS.HasFeature("pc") || OS.HasFeature("web_linuxbsd") || OS.HasFeature("web_macos") || OS.HasFeature("web_windows"))
        {
            InputSchemeHelper.RequestSchemeType(InputSchemeType.KBM);
        }
        else if (OS.HasFeature("web_android") || OS.HasFeature("web_ios") || OS.HasFeature("android") || OS.HasFeature("ios") || OS.HasFeature("mobile"))
        {
            InputSchemeHelper.RequestSchemeType(InputSchemeType.Touch);
        }
        else
        {
            InputSchemeHelper.RequestSchemeType(InputSchemeType.Gamepad);
        }
        delay = 1.0;
    }

    public override void _Process(double delta)
    {
        //delay is used to prevent erronious button presses during boot like phantom trigger pulls
        if (delay > 0.0f)
        {
            delay -= delta;
            if (delay <= 0.0)
            {
                //we're ready after a small time
                ready = true;
            }
        }
#if IMGUI
        ImGui.Begin("Input Scheme");
        ImGui.Text($"Input Scheme Type: {InputSchemeChooser.SchemeType}");
        ImGui.End();
#endif
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (!ready) { return; }

        if (@event is InputEventKey keyInput)
        {
            //don't consider dummy device 42069
            if (keyInput.Device != 42069)
            {
                InputSchemeHelper.RequestSchemeType(InputSchemeType.KBM);
            }
        }
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (!ready) { return; }

        if (mouseMovementSwitchesScheme)
        {
            if (@event is InputEventMouseMotion mouseMoved)
            {
                //don't consider dummy device 42069
                if (mouseMoved.Device != 42069)
                {
                    InputSchemeHelper.RequestSchemeType(InputSchemeType.KBM);
                }
            }
        }
        if (@event is InputEventJoypadButton joyButton)
        {
            //don't consider dummy device 42069
            if (joyButton.Device != 42069)
            {
                InputSchemeHelper.RequestSchemeType(InputSchemeType.Gamepad);
            }
        }
        if (@event is InputEventJoypadMotion joyMotion)
        {
            //don't consider dummy device 42069
            if (joyMotion.Device != 42069)
            {
                InputSchemeHelper.RequestSchemeType(InputSchemeType.Gamepad);
            }
        }
    }
}
