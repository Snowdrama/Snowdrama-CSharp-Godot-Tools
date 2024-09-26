using Godot;
using Godot.Collections;
using System;

public partial class InteractiveMusicManager : AudioStreamPlayer
{
	public static InteractiveMusicManager instance;

	AudioStreamInteractive stream;

	Array<string> trackNames = new Array<string>();
	public override void _EnterTree()
    {
        GD.PrintErr("InteractiveMusicManager is Entering");
        if (instance != null)
		{
            instance.QueueFree();
        }
        instance = this;
    }

    public override void _ExitTree()
    {
        GD.PrintErr("InteractiveMusicManager is ExitingTree");
    }

    [Export] public string trackTarget { get; private set; }
    [Export] public string currentTrack { get; private set; }

    public override void _Ready()
    {
        if (stream == null)
        {
            stream = (AudioStreamInteractive)this.Stream;
        }

		for (int i = 0; i < stream.ClipCount; i++)
		{
            trackNames.Add(stream.GetClipName(i));
        }

		
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (currentTrack != trackTarget && trackNames.Contains(trackTarget))
		{
			currentTrack = trackTarget;
            this.Set("parameters/switch_to_clip", trackTarget);
		}

        if(this.StreamPaused)
        {
            this.StreamPaused = false;
        }
    }

	public static void ChangeToTrack(string name)
	{
        instance.trackTarget = name;
    }
}
