using Godot;


[GlobalClass]
public partial class UIRoute : Node
{
    [Export] private UIRoutingSystem _routingSystem;
    [Export] private string routeSegment;
    [Export] private CanvasLayer mainContent;
    [Export] private Control focusOnVisible;
    [Export] private bool startEnabled = false;
    public override void _Ready()
    {
        if(_routingSystem == null)
        {
            _routingSystem = this.GetParent<UIRoutingSystem>();
        }

        _routingSystem.GetRouter().RegisterRoute(routeSegment, this);
        if (startEnabled)
        {
            _routingSystem.GetRouter().OpenRoute(routeSegment);
            mainContent.Show();
        }
        else
        {
            mainContent.Hide();
        }
    }
    public override void _ExitTree()
    {
        _routingSystem.GetRouter().UnregisterRoute(routeSegment);
    }
    public UIRouter GetRouter()
    {
        return _routingSystem.GetRouter();
    }
    public void OpenRoute()
    {
        // TODO: Force the selection to this for gamepads.
        if (focusOnVisible != null)
        {
            focusOnVisible?.GrabFocus();
        }

        mainContent.Show();
    }

    public void CloseRoute()
    {
        mainContent.Hide();
    }

    public override void _Process(double delta)
    {
        if (mainContent.Visible)
        {
            //check if we have a focus, if somehow we lose that focus grab it again
            if(GetViewport().GuiGetFocusOwner() == null)
            {
                if (focusOnVisible != null)
                {
                    focusOnVisible?.GrabFocus();
                }
            }
        }
    }
}