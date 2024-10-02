using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class CursorManager : Node
{
    public static CursorManager Instance;

    public static List<string> visibleSources = new List<string>();
    [Export] Input.MouseModeEnum MouseStateOnEnterTree;
    [Export] Input.MouseModeEnum MouseStateOnExitTree;
    [Export] Input.MouseModeEnum MouseStateDefault;
    [Export] Input.MouseModeEnum MouseStateWhenSourcesActive;
    public override void _EnterTree()
    {
        this.ProcessMode = ProcessModeEnum.Always;

        Input.MouseMode = MouseStateOnEnterTree;
        ClearMenus();
    }

    public override void _ExitTree()
    {
        Input.MouseMode = MouseStateOnExitTree;
        ClearMenus();
    }


    public override void _Process(double delta)
    {
        if(visibleSources.Count > 0)
        {
            Input.MouseMode = MouseStateWhenSourcesActive;
        }
        else
        {
            Input.MouseMode = MouseStateDefault;
        }
    }

    public static void MenuOpen(string menuName)
    {
        if (!visibleSources.Contains(menuName))
        {
            visibleSources.Add(menuName);
        }
    }
    public static void MenuClose(string menuName)
    {
        if (visibleSources.Contains(menuName))
        {
            visibleSources.Remove(menuName);
        }
    }

    public static void ClearMenus()
    {
        visibleSources.Clear();
    }
}