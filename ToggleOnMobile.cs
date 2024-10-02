using Godot;
using System;

public partial class ToggleOnMobile : Node
{
    [Export] Node target;
    [Export] ProcessModeEnum processTypeDefault = ProcessModeEnum.Inherit;
    [Export] bool visibilityDefault = true;
    [Export] ProcessModeEnum processTypeOnMobile = ProcessModeEnum.Disabled;
    [Export] bool visibilityOnMobile = false;
    public override void _Ready()
    {
        if(target == null)
        {
            return;
        }
        //are we on something with a primary touch interface?
        if (OS.HasFeature("mobile") || OS.HasFeature("web_android") || OS.HasFeature("web_ios"))
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
            if (target is CanvasLayer canvas)
            {
                canvas.Visible = visibilityOnMobile;
            }
        }
        else
        {
            target.ProcessMode = processTypeDefault;
            if (target is Node2D node2D)
            {
                node2D.Visible = visibilityDefault;
            }
            if (target is Node2D node3D)
            {
                node3D.Visible = visibilityDefault;
            }
            if (target is CanvasLayer canvas)
            {
                canvas.Visible = visibilityDefault;
            }
        }
    }
}