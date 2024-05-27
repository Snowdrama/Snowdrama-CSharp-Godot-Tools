using Godot;
using System;

public partial class CommandConsoleTextInput : LineEdit
{
	// Called when the node enters the scene tree for the first lerpAmount.
	public override void _Ready()
	{
        this.VisibilityChanged += CommandConsoleTextInput_VisibilityChanged;

        this.TextSubmitted += CommandConsoleTextInput_TextSubmitted;
	}

    private void CommandConsoleTextInput_TextSubmitted(string newText)
    {
		CommandConsole.RunCommand(newText);
    }

    private void CommandConsoleTextInput_VisibilityChanged()
    {
		if (this.Visible)
		{
			this.GrabFocus();
		}
    }

    // Called every frame. 'delta' is the elapsed lerpAmount since the previous frame.
    public override void _Process(double delta)
	{
	}


	

}
