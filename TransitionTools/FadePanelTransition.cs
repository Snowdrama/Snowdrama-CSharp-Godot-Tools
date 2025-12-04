using Godot;

[GlobalClass]
public partial class FadePanelTransition : Transition
{
    [Export] Panel panel;
    StyleBox styleBox;

    [Export] Color fadeColor;
    public override void _Ready()
    {
        styleBox = panel.GetThemeStylebox("panel");

        fadeColor.A = 0;
        styleBox.Set("bg_color", fadeColor);

        panel.MouseFilter = Control.MouseFilterEnum.Ignore;
        panel.ProcessMode = ProcessModeEnum.Always;
    }
    public override void SetTransitionValue(float transitionAmount)
    {
        if (transitionAmount > 0)
        {
            panel.Visible = true;
        }
        else
        {
            panel.Visible = false;
        }
        fadeColor.A = transitionAmount;
        styleBox.Set("bg_color", fadeColor);
    }
}