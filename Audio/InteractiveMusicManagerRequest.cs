using Godot;
using System;

public partial class InteractiveMusicManagerRequest : Node
{
	[Export] string trackName;
	public override void _Ready()
	{
		InteractiveMusicManager.ChangeToTrack(trackName);
	}
}
