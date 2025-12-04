using Godot;
using System;

[GlobalClass]
public partial class FadeColorRectTransition : Transition
{
    [Export] ColorRect colorRect;
    [Export] Color fadeColor;

    public override void _Ready()
    {
        base._Ready();
        SetTransitionValue(0);
    }
    public override void SetTransitionValue(float transitionAmount)
    {
        if (transitionAmount > 0)
        {
            colorRect.Visible = true;
        }
        else
        {
            colorRect.Visible = false;
        }
        fadeColor.A = transitionAmount;
        colorRect.Modulate = fadeColor;
    }
}
