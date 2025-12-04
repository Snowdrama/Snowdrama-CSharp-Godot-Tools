using Godot;

[GlobalClass]
public partial class ShaderMaterialTransition : Transition
{
    [Export] Material shaderMaterialReference;
    [Export] string shaderProperty = "shader_parameter/progress";
    [Export] Vector2 progressRange = new Vector2(0, 1);

    public override void _Ready()
    {
        base._Ready();
        shaderMaterialReference.Set(shaderProperty, Mathf.Lerp(progressRange.X, progressRange.Y, 0));
    }
    public override void SetTransitionValue(float transitionAmount)
    {
        shaderMaterialReference.Set(shaderProperty, Mathf.Lerp(progressRange.X, progressRange.Y, transitionAmount));
    }
}
