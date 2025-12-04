using Godot;

public partial class UIRouterPause : Node
{

    [Export] string pauseEvent = "Pause";
    [Export] string cancelKey = "Cancel";
    [Export] UIRouter router;

    bool _paused = false;
    bool Paused
    {
        get => _paused;
        set
        {
            Debug.Log($"Pause Chagned? {_paused} != {value}");
            //only when changed!
            if (_paused != value)
            {
                _paused = value;
                GetTree().Paused = _paused;
                CursorManager.MenuOpen("PauseMenu");
                if (!router.IsRouteOpen("pause"))
                {
                    router.OpenRoute("pause");
                }
                PauseManager.RequestPause(this);
            }
            else
            {
                CursorManager.MenuClose("PauseMenu");
                if (router.OpenRouteCount() > 0)
                {
                    router.CloseAll();
                }
                PauseManager.RequestUnpause(this);
            }
        }
    }

    public override void _EnterTree()
    {
        ProcessMode = Node.ProcessModeEnum.Always;
    }

    public override void _ExitTree()
    {
        this.Paused = false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (GetTree().Paused && router.OpenRouteCount() <= 0)
        {
            Paused = false;
        }

        if (!Paused && GetTree().Paused)
        {
            Paused = false;
        }
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed(pauseEvent))
        {
            Debug.Log("Pause Key Pressed!");
            Paused = !Paused;
        }




        if (@event.IsActionPressed(cancelKey))
        {
            router.Back();
        }
    }
}
