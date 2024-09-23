using Godot;
public partial class UIRoute : Node
{
    [Export] private UIRouter _router;
    [Export] private string routeSegment;
    [Export] private CanvasLayer mainContent;
    [Export] private Control focusOnVisible;
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
        if (focusOnVisible != null)
        {
            focusOnVisible?.GrabFocus();
        }
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