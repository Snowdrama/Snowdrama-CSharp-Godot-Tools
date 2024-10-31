using Godot;
using System;

public partial class OpenURLButton : Button
{

    [Export]
    string urlToOpen;
	public override void _Ready()
	{
        this.Pressed += OpenURLButton_Pressed;
	}

    private void OpenURLButton_Pressed()
    {
        OS.ShellOpen(urlToOpen);
    }
}
