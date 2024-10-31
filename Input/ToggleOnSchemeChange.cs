using Godot;
using System;

public partial class ToggleOnSchemeChange : Node
{
    [Export] Node target;
    [Export] ProcessModeEnum processTypeKBM = ProcessModeEnum.Inherit;
    [Export] bool visibilityKBM = true;
    [Export] ProcessModeEnum processTypeTouch = ProcessModeEnum.Inherit;
    [Export] bool visibilityTouch = true;
    [Export] ProcessModeEnum processTypeGamepad = ProcessModeEnum.Inherit;
    [Export] bool visibilityGamepad = true;
    InputSchemeChangedMessage inputSchemeChanged;
    public override void _EnterTree()
    {
        inputSchemeChanged = Messages.Get<InputSchemeChangedMessage>();
        inputSchemeChanged.AddListener(SchemeChanged);
    }
    public override void _ExitTree()
    {
        inputSchemeChanged.RemoveListener(SchemeChanged);
        Messages.Return<InputSchemeChangedMessage>();
    }
    public void SchemeChanged(InputSchemeType type)
    {
        GD.Print($"ToggleOnSchemeChanged Type: {type}");
        switch (type)
        {
            case InputSchemeType.KBM:
                SetVisibility(visibilityKBM);
                SetProcess(processTypeKBM);
                break;
            case InputSchemeType.Gamepad:
                SetVisibility(visibilityGamepad);
                SetProcess(processTypeGamepad);
                break;
            case InputSchemeType.Touch:
                SetVisibility(visibilityTouch);
                SetProcess(processTypeTouch);
                break;
        }
    }

    private void SetVisibility(bool vis)
    {
        if (target is Node2D node2D)
        {
            node2D.Visible = vis;
        }
        if (target is Node2D node3D)
        {
            node3D.Visible = vis;
        }
        if (target is Control control)
        {
            control.Visible = vis;
        }
        if (target is CanvasLayer canvas)
        {
            canvas.Visible = vis;
        }
    }
    private void SetProcess(ProcessModeEnum type)
    {
        target.ProcessMode = type;
    }
}
