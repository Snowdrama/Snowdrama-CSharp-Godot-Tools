using Godot;
using System;

public partial class UIRouteBackButton : UIBaseRouterButton
{
    public override void _Ready()
    {
        this.Connect(SignalName.Pressed, Callable.From(() => { Router.Back(); }));
    }
}
