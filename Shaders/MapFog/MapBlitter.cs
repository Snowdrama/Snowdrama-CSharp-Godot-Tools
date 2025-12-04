using Godot;
using System;

public partial class MapBlitter : Node2D
{
    private float time = 1.0f;
    private float distanceTraveled = 0.0f;
    private Vector2 lastPosition;

    public override void _Ready()
    {
        base._Ready();
        Messages.GetOnce<BlitRevealToMapMessage>().Dispatch(
            this.GlobalPosition.RoundToInt(),
            "MapTest"
        );
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        distanceTraveled += lastPosition.DistanceTo(this.GlobalPosition);
        lastPosition = this.GlobalPosition;

        if (distanceTraveled > 5.0f)
        {
            distanceTraveled -= 5.0f;
            Messages.GetOnce<BlitRevealToMapMessage>().Dispatch(
                this.GlobalPosition.RoundToInt(),
                "MapTest"
            );
        }
    }
}
