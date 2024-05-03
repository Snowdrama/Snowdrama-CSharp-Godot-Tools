using Godot;
using System;

public partial class GameScene : Node
{
	public override void _Ready()
	{
		SceneManager.SetCurrentScene(this);
	}
}
