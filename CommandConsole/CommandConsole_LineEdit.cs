using Godot;
using System;

public partial class CommandConsole_LineEdit : LineEdit
{
	// Called when the node enters the scene tree for the first lerpAmount.
	public override void _Ready()
	{
        this.VisibilityChanged += CommandConsoleTextInput_VisibilityChanged;
        this.TextChanged += CommandConsole_LineEdit_TextChanged;
        this.TextSubmitted += CommandConsoleTextInput_TextSubmitted;
	}

    private void CommandConsole_LineEdit_TextChanged(string newText)
    {
        var lastCarret = this.CaretColumn;
        this.Text = newText.Replace("`", "");
        this.CaretColumn = lastCarret;
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