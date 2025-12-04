using Godot;
using Snowdrama.Core;
using System;

public partial class SquishSpring2DTester : Node
{
    [Export] private SquishSpring2D squishSpring;

    [Export] private Vector2 xRange = new Vector2(-1, 1);
    [Export] private Vector2 yRange = new Vector2(-1, 1);
    public override void _Process(double delta)
    {
        base._Process(delta);

    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey keyPressed)
        {
            if (keyPressed.Pressed && keyPressed.Keycode == Key.Space)
            {
                var x = RandomAndNoise.RandomRange(xRange);
                var y = RandomAndNoise.RandomRange(yRange);
                squishSpring.SpringSquish(new Vector2(x, y));
            }
        }
    }
}
