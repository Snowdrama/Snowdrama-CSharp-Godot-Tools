using Godot;
using System;

public partial class UIRouterPause : Node
{
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
                if (!router.IsRouteOpen("pause"))
                {
                    router.OpenRoute("pause");
                }
            }
            else
            {
                if (router.OpenRouteCount() > 0)
                {
                    router.CloseAll();
                }
            }
        }
    }

    public override void _Ready()
    {
        ProcessMode = Node.ProcessModeEnum.Always;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        if(GetTree().Paused && router.OpenRouteCount() <= 0)
        {
            GetTree().Paused = false;
        }

        if (!Paused && GetTree().Paused)
        {
            GetTree().Paused = false;
        }
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("Pause"))
        {
            Paused = !Paused;
        }
        else if (@event is InputEventKey eventKey)
        {
            if(eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
                Paused = !Paused;
            }
        }
    }
}
