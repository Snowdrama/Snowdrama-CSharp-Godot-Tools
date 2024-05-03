using Godot;
using System;

public partial class FadeImageTransition : Transition
{
    [Export] TextureRect textureRect;

    [Export] Color fadeColor;
    public override void _Ready()
    {
        fadeColor.A = 0;
        textureRect.Modulate = fadeColor;

        textureRect.MouseFilter = Control.MouseFilterEnum.Ignore;
        textureRect.ProcessMode = ProcessModeEnum.Always;
        this.ProcessMode = ProcessModeEnum.Always;
    }
    public override void SetTransitionValue(float transitionAmount)
    {
        if (transitionAmount > 0)
        {
            textureRect.Visible = true;
        }
        else
        {
            textureRect.Visible = false;
        }
        fadeColor.A = transitionAmount;
        textureRect.Modulate = fadeColor;
    }
}
