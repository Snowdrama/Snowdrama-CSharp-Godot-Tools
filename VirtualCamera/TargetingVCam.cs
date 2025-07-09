using Godot;

[Tool]
public partial class TargetingVCam : VirtualCamera2D
{
    [Export] Node2D? target;
    [Export] float distanceSpeed = 5;
    [Export] float linearSpeed = 2.5f;

    Vector2 _framing;
    [Export]
    Vector2 Framing
    {
        get
        {
            return _framing;
        }
        set
        {
            _framing = value;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (target != null)
        {
            var distance = this.Position.DistanceTo(target.Position);
            this.Position = this.Position.MoveToward(target.Position, ((distanceSpeed * distance) + (linearSpeed)) * (float)delta);
        }
        else
        {
            Debug.LogError($"target in VCam is null");
        }
    }

    public override void _Draw()
    {
        base._Draw();
    }
}
