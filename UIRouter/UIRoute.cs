using Godot;


[GlobalClass]
public partial class UIRoute : Node
{
    [Export] private UIRouter? _router;
    [Export] private string routeSegment = "";
    [Export] private Control? mainContent;
    [Export] private Control? focusOnVisible;
    [Export] private bool startEnabled = false;
    public override void _Ready()
    {
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

    public override void _EnterTree()
    {
        ProcessMode = Node.ProcessModeEnum.Always;
        _router?.RegisterRoute(routeSegment, this);
    }

    public override void _ExitTree()
    {
        _router.UnregisterRoute(routeSegment);
    }
    public UIRouter GetRouter()
    {
        return _router;
    }
    public void OpenRoute()
    {
        // TODO: Force the selection to this for gamepads.
        if (focusOnVisible != null)
        {
            focusOnVisible?.SetBlockSignals(true);
            focusOnVisible?.GrabFocus();
            focusOnVisible?.SetBlockSignals(false);
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
            if (GetViewport().GuiGetFocusOwner() == null)
            {
                if (focusOnVisible != null)
                {
                    focusOnVisible.SetBlockSignals(true);
                    focusOnVisible?.GrabFocus();
                    focusOnVisible.SetBlockSignals(false);
                }
            }
        }
    }
}