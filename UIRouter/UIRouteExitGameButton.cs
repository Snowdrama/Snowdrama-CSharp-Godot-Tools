using Godot;
using System;

public partial class UIRouteExitGameButton : UIBaseRouterButton
{
    public override void _Ready()
    {
        this.Connect(SignalName.Pressed, Callable.From(() => {
            GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
        }));
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
            GetTree().Quit(); // default behavior
    }
}
