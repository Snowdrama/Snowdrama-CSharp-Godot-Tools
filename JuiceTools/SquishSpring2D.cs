using Godot;
using Snowdrama.Spring;
using System;

/// <summary>
/// the idea here is that implementing tweens and springs in a script uses
/// a bunch of space and is annoying so this implements all the stuff
/// as a middleman so all you nee to add to your main script is some 
/// basic one line function calls.
/// </summary>
[GlobalClass]
public partial class SquishSpring2D : Node2D
{
    [Export] private SpringConfigurationResource squishSpringConfig;
    private Spring2D squishSpring;
    [Export] private float squishStrength;
    public override void _Ready()
    {
        base._Ready();
        squishSpring = new Spring2D(squishSpringConfig.Config, new Vector2(1.0f, 1.0f));
    }
    public void SpringSquish(Vector2 direction)
    {
        //input direction will be a normalized vector
        //positive is a stretch and, negative is squish?
        squishSpring.Velocity = (direction.Normalized() * squishStrength);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        squishSpring.Update(delta);
        this.GlobalScale = squishSpring.Value;
    }
}
