using Godot;
using Godot.Collections;
using Snowdrama.Spring;

/// <summary>
/// Used for doing stuff like "Scale on hover" and this middle class should help
/// make implementation of that easier for controls
/// </summary>
[GlobalClass]
public partial class TweenTargetControl : Control
{
    [Export] private SpringConfigurationResource squishSpringConfig;
    private Spring2D squishSpring;

    [Export]
    private Array<Vector2> targets = new Array<Vector2>()
    {
        new Vector2(1.0f, 1.0f),
        new Vector2(0.5f, 0.5f),
        new Vector2(1.5f, 1.5f),
    };

    public override void _Ready()
    {
        base._Ready();
        squishSpring = new Spring2D(squishSpringConfig.Config, targets[0]);
    }

    public void GoToTarget(int targetId)
    {
        if (targetId >= 0 && targetId < targets.Count)
        {
            squishSpring.Target = targets[targetId];
        }
    }

    public void TweenTo(Vector2 size)
    {
        squishSpring.Target = size;
    }

    public void TweenTo(float size)
    {
        squishSpring.Target = new Vector2(size, size);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        squishSpring.Update(delta);
        this.Scale = squishSpring.Value;
    }
}
