using Godot;
using System;

public partial class ToggleOnWeb : Node
{
    [Export] Node target;
    [Export] ProcessModeEnum processTypeOnMobile = ProcessModeEnum.Disabled;
    [Export] bool visibilityOnMobile = false;
    public override void _Ready()
    {
        if (target == null)
        {
            return;
        }

        if (OS.HasFeature("web"))
        {
            target.ProcessMode = processTypeOnMobile;
            if (target is Node2D node2D)
            {
                node2D.Visible = visibilityOnMobile;
            }
            if (target is Node2D node3D)
            {
                node3D.Visible = visibilityOnMobile;
            }
        }
    }
}
