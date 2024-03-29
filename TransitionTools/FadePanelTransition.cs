﻿using Godot;
public partial class FadePanelTransition : Transition
{
    StyleBoxFlat styleBox;

    [Export] Color fadeColor;
    public override void _Ready()
    {
        styleBox = (StyleBoxFlat)GetThemeStylebox("panel");

        fadeColor.A = 0;
        styleBox.Set("bg_color", fadeColor);

        this.MouseFilter = MouseFilterEnum.Ignore;
        this.ProcessMode = ProcessModeEnum.Always;
    }
    public override void SetTransitionValue(float transitionAmount)
    {
        if(transitionAmount > 0)
        {
            this.Visible = true;
        }
        else
        {
            this.Visible = false;
        }
        fadeColor.A = transitionAmount;
        styleBox.Set("bg_color", fadeColor);
    }
}