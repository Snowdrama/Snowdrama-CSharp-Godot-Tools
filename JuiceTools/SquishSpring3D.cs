using Godot;
using Snowdrama.Spring;
/// <summary>
/// the idea here is that implementing tweens and springs in a script uses
/// a bunch of space and is annoying so this implements all the stuff
/// as a middleman so all you nee to add to your main script is some 
/// basic one line function calls.
/// </summary>
[GlobalClass]
public partial class SquishSpring3D : Node3D
{
    [Export] private SpringConfigurationResource squishSpringConfig;
    private Spring3D squishSpring;
    [Export] private float squishStrength;
    public override void _Ready()
    {
        base._Ready();
        squishSpring = new Spring3D(squishSpringConfig.Config, new Vector3(1.0f, 1.0f, 1.0f));
    }
    public void SpringSquish(Vector3 direction)
    {
        //input direction will be a normalized vector
        //positive is a stretch and, negative is squish?
        squishSpring.Velocity = (direction.Normalized() * squishStrength);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        squishSpring.Update(delta);
        this.Scale = squishSpring.Value;
    }
}
