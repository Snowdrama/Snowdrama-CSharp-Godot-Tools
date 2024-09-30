using Godot;
using System;

public partial class UIRouteCloseAllButton : UIBaseRouterButton
{
    public override void _Ready()
    {
        this.Connect(SignalName.Pressed, Callable.From(() => { Router.CloseAll(); }));
    }
}
