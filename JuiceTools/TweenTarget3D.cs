using Godot;
using Godot.Collections;
using Snowdrama.Spring;

[GlobalClass]
public partial class TweenTarget3D : Node3D
{
    [Export] private SpringConfigurationResource squishSpringConfig;
    private Spring3D squishSpring;

    [Export]
    private Array<Vector3> targets = new Array<Vector3>()
    {
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(1.5f, 1.5f, 1.5f),
    };

    public override void _Ready()
    {
        base._Ready();
        squishSpring = new Spring3D(squishSpringConfig.Config, targets[0]);
    }

    public void GoToTarget(int targetId)
    {
        if (targetId >= 0 && targetId < targets.Count)
        {
            squishSpring.Target = targets[targetId];
        }
    }

    public void TweenTo(Vector3 size)
    {
        squishSpring.Target = size;
    }

    public void TweenTo(float size)
    {
        squishSpring.Target = new Vector3(size, size, size);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        squishSpring.Update(delta);
        this.Scale = squishSpring.Value;
    }
}
