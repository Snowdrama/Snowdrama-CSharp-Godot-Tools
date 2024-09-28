using Godot;
using System;

public partial class UIRouteGoToSceneButton : Button
{
    [Export] UIRouter _router;
    [Export] string sceneName;
    public override void _Ready()
    {
        this.Connect(SignalName.Pressed, Callable.From(() => {
            SceneManager.LoadScene(sceneName);
        }));
    }
}
