using Godot;
using System;
using System.ComponentModel;

public partial class UIRouteOpenButton : UIBaseRouterButton
{
    [Export] string routeToOpen;
    [Export, Description("If you open a route exclusively, you remove all router history and calling back will close the UI")] 
    bool exclusive = false;
    public override void _Ready()
    {
        base._Ready();
        this.Connect(SignalName.Pressed, Callable.From(() => { 
            if(exclusive){
                Router.OpenRouteExclusive(routeToOpen);
            }
            else{
                Router.OpenRoute(routeToOpen);
            }
        }));
    }
}
