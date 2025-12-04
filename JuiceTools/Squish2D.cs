using Godot;
using Snowdrama.Spring;
using System;

/// <summary>
/// the idea here is that implementing tweens and springs in a script uses
/// a bunch of space and is annoying so this implements all the stuff
/// as a middleman so all you nee to add to your main script is some 
/// basic one line function calls.
/// </summary>
public partial class Squish2D : Node
{
    [Export] private SpringConfiguration squishSpringConfig;
    private Spring2D squishSpring;

    public void SpringSquish(Vector2 direction)
    {
        squishSpring.Velocity = direction;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        squishSpring.Update(delta);
    }
}
