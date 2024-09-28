using Godot;
using System;

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
            _paused = value;
            GetTree().Paused = _paused;
            if (_paused)
            {
                CursorManager.MenuOpen("PauseMenu");
                if (!router.IsRouteOpen("pause"))
                {
                    router.OpenRoute("pause");
                }
            }
            else
            {
                CursorManager.MenuClose("PauseMenu");
                if (router.OpenRouteCount() > 0)
                {
                    router.CloseAll();
                }
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

        if(GetTree().Paused && router.OpenRouteCount() <= 0)
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
            Paused = !Paused;
        }

        
        

        if (@event.IsActionPressed(cancelKey))
        {
            router.Back();
        }
    }
}
