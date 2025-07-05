using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class PauseManager : Node
{
    static List<Node> pauseSources = new List<Node>();

    static PauseManager instance;

    static bool _paused;
    public static bool Paused
    {
        get { return _paused; }
        private set
        {
            _paused = value;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        if (instance != null)
        {
            QueueFree();
            return;
        }
        this.ProcessMode = ProcessModeEnum.Always;
        instance = this;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (pauseSources.Count > 0 && !Paused)
        {
            Paused = true;
            GetTree().Paused = true;
        }
        else if (pauseSources.Count <= 0)
        {
            Paused = false;
            GetTree().Paused = false;
        }
        pauseSources = pauseSources.Where(x => x != null).ToList();
    }

    public static void RequestPause(Node source)
    {
        if (!pauseSources.Contains(source))
        {
            pauseSources.Add(source);
        }
    }

    public static void RequestUnpause(Node source)
    {
        if (pauseSources.Contains(source))
        {
            pauseSources.Remove(source);
        }
    }
}
