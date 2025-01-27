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
    [Export] Input.MouseModeEnum MouseStateWhenGamepad;

    [Export] bool disableCapture = false;
    public override void _EnterTree()
    {
        this.ProcessMode = ProcessModeEnum.Always;
        if (!disableCapture)
        {
            Input.MouseMode = MouseStateOnEnterTree;
            ClearMenus();
        }
    }

    public override void _ExitTree()
    {
        if (!disableCapture)
        {
            Input.MouseMode = MouseStateOnExitTree;
            ClearMenus();
        }
    }


    public override void _Process(double delta)
    {
        if (!disableCapture)
        {
            var targetNewMode = Input.MouseMode;

            if (InputSchemeChooser.SchemeType == InputSchemeType.KBM)
            {
                if (visibleSources.Count > 0)
                {
                    targetNewMode = MouseStateWhenSourcesActive;
                }
                else
                {
                    targetNewMode = MouseStateDefault;
                }
            }
            else
            {
                targetNewMode = MouseStateWhenGamepad;
            }

            if (Input.MouseMode != targetNewMode)
            {
                if (InputSchemeChooser.SchemeType == InputSchemeType.KBM)
                {
                    if (visibleSources.Count > 0)
                    {
                        Debug.Log($"There's {visibleSources.Count} Visible sources so mouse is {MouseStateWhenSourcesActive}");
                    }
                    else
                    {
                        Debug.Log($"No visible menus so mouse state is: {MouseStateDefault}");
                    }
                }
                else
                {
                    Debug.Log($"Gamepad is being used so mouse is now: {MouseStateWhenGamepad}");
                }

                Input.MouseMode = targetNewMode;
            }
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