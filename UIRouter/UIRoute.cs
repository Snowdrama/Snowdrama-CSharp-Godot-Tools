using Godot;
public partial class UIRoute : Node
{
    [Export] private UIRouter _router;
    [Export] private string routeSegment;
    [Export] private CanvasLayer mainContent;
    [Export] private Control selectOnRouteOpen;
    [Export] private bool startEnabled = false;
    public override void _Ready()
    {
        if(_router == null)
        {
            _router = this.GetParent<UIRouter>();
        }
        _router.RegisterRoute(routeSegment, this);
        if (startEnabled)
        {
            _router.OpenRoute(routeSegment);
            mainContent.Show();
        }
        else
        {
            mainContent.Hide();
        }
    }
    public override void _ExitTree()
    {
        _router.UnregisterRoute(routeSegment);
    }

    public void OpenRoute()
    {
        mainContent.Show();

        // TODO: Force the selection to this for gamepads.
        // if (selectOnRouteOpen)
        // {
        //     selectOnRouteOpen.Select();
        // }
    }

    public void CloseRoute()
    {
        mainContent.Hide();
    }
}