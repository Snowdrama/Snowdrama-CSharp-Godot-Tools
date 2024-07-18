using Godot;
using System;

public partial class CommandConsole_LineEdit : LineEdit
{
	// Called when the node enters the scene tree for the first lerpAmount.
	public override void _Ready()
	{
        this.VisibilityChanged += CommandConsoleTextInput_VisibilityChanged;

        this.TextSubmitted += CommandConsoleTextInput_TextSubmitted;
	}

    private void CommandConsoleTextInput_TextSubmitted(string newText)
    {
		if(CommandConsole.RunCommand(newText))
		{
			this.Text = "";
		}
    }

    private void CommandConsoleTextInput_VisibilityChanged()
    {
		if (this.Visible)
		{
			this.GrabFocus();
		}
    }
}
