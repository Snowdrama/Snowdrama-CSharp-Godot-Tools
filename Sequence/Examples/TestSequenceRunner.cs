using Godot;
using System;

public partial class TestSequenceRunner : Node
{
	[Export] Sequence testSequence;


	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
    public override void _Input(InputEvent @event)
    {
		if(@event is InputEventKey key)
        {
            if(key.Pressed)
            {
                if (key.Keycode == Key.F1)
                {
                    testSequence.PlaySequence();
                }
                if (key.Keycode == Key.F2)
                {
                    testSequence.PlaySequence(true);
                }
                if (key.Keycode == Key.Space)
                {
                    testSequence.AdvanceSequence();
                }
            }
        }
    }
}
