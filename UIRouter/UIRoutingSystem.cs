using Godot;

[GlobalClass]
public partial class UIRoutingSystem : Node
{
    [Export] private UIRouter _uiRouter;
    public UIRouter GetRouter()
    {
        return _uiRouter;
    }
}
