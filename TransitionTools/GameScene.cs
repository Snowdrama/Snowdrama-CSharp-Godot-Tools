using Godot;
using System;

[GlobalClass]
public partial class GameScene : Node
{
	public override void _Ready()
	{
		SceneManager.SetCurrentScene(this);
		GD.Print($"Current Locale {TranslationServer.GetLocale()}");
	}
}
