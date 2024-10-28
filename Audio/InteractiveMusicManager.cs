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
        if (instance != null)
		{
            instance.QueueFree();
        }
        instance = this;

        this.ProcessMode = ProcessModeEnum.Always;
    }

    public override void _ExitTree()
    {
    }

    [Export] public string trackTargetName { get; private set; }
    [Export] public string trackOverrideName { get; private set; }

    [Export] public bool applyTrackOverride { get; private set; }
    [Export] public string currentTrackName { get; private set; }

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

        //if (!this.Playing)
        //{
        //    this.Play();
        //}
    }

    // Called every frame. 'delta' is the elapsed UpdateTimeMax since the previous frame.
    public override void _Process(double delta)
	{
        if (applyTrackOverride)
        {
            if(currentTrackName != trackOverrideName && trackNames.Contains(trackOverrideName))
            {
                currentTrackName = trackOverrideName;
                this.Set("parameters/switch_to_clip", trackOverrideName);
            }
        }
        else
        {
            if (currentTrackName != trackTargetName && trackNames.Contains(trackTargetName))
            {
                currentTrackName = trackTargetName;
                this.Set("parameters/switch_to_clip", trackTargetName);
            }
        }

        //we don't ever want the music to pause
        if(this.StreamPaused)
        {
            this.StreamPaused = false;
        }
    }

    public static void ChangeToTrack(string name, bool startPlaying = true)
    {
        instance.trackTargetName = name;
        if (startPlaying && !instance.Playing)
        {
            instance.Play();
        }
    }
    public static void TemporaryTrackOverride(string name, bool overrideTrack, bool startPlaying = true)
    {
        instance.trackOverrideName = name;
        instance.applyTrackOverride = overrideTrack;
        if (startPlaying && !instance.Playing)
        {
            instance.Play();
        }
    }
}
