using Godot;
using System;

[GlobalClass]
public partial class UIBaseRouterButton : Button
{
    [Export] protected UIRoute ParentRoute;
    protected UIRouter Router
    {
        get
        {
            return ParentRoute.GetRouter();
        }
    }
}
