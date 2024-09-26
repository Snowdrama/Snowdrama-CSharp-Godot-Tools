using Godot;
using System;

public partial class UIRouteGoToSceneButton : Button
{
    [Export] UIRouter _router;
    [Export] string sceneName;
    public override void _Ready()
    {
        GD.Print($"Connecting Button to go to scene: {sceneName}");
        this.Connect(SignalName.Pressed, Callable.From(() => {
            GD.Print("Butts!");
            SceneManager.LoadScene(sceneName);
        }));
    }
}
