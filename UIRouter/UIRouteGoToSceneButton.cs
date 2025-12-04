using Godot;
using System;

public partial class UIRouteGoToSceneButton : UIBaseRouterButton
{
    [Export] string sceneName;
    public override void _Ready()
    {
        base._Ready();
        this.Connect(SignalName.Pressed, Callable.From(() => {
            Debug.LogWarning($"Loading Scene:{sceneName}");
            SceneManager.LoadScene(sceneName);
        }));

        if (Disabled)
        {
            this.FocusMode = FocusModeEnum.None;
        }
    }
}
