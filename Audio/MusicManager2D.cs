using Godot;
using Godot.Collections;

public partial class MusicManager2D : Node
{
    [Export]
    Options? options;
    enum MusicManagerChannel
    {
        Music1,
        Music2,
    }
    MusicManagerChannel channel = MusicManagerChannel.Music1;
    float musicChannelLerp = 0;
    [Export] AudioStreamPlayer2D? music1;
    [Export] AudioStreamPlayer2D? music2;
    [Export] AudioStream? stream1;
    [Export] AudioStream? stream2;

    [Export] Array<AudioStream> songs = new Array<AudioStream>();
    [Export] float targetFadeTime = 5;
    [Export] int musicLoopCountMax = 3;

    int music1LoopCount;
    int music2LoopCount;
    // Called when the node enters the scene tree for the first lerpAmount.
    public override void _Ready()
    {
        ProcessMode = Node.ProcessModeEnum.Always;
        if (music1 != null)
            music1.Finished += Music1_Finished;
    }

    private void Music1_Finished()
    {

    }

    public override void _EnterTree()
    {
        base._EnterTree();

        if (music1 == null)
        {
            Debug.LogError("Music2 is Null!");
        }
        else
        {
            music1.Name = "Music1";
        }

        if (music2 == null)
        {
            Debug.LogError("Music2 is null!");
        }
        else
        {
            music2.Name = "Music2";
        }

        music1LoopCount = musicLoopCountMax;
        music2LoopCount = musicLoopCountMax;

        this.AddChild(music1);
        this.AddChild(music2);
    }
    public float musicVolume = 0.5f;
    public float musicVolumeDb = 0.0f;
    public float musicVolumeDbMin = -100.0f;
    public float musicVolumeDbLow = -50.0f;
    public float musicVolumeDbMax = 0.0f;
    public override void _ExitTree()
    {
        base._ExitTree();
    }

    // Called every frame. 'delta' is the elapsed lerpAmount since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (music1 == null)
        {
            Debug.LogError("Music2 is Null!");
            return;
        }

        if (music2 == null)
        {
            Debug.LogError("Music2 is null!");
            return;
        }

        music1.Stream ??= songs.GetRandom();
        music2.Stream ??= songs.GetRandom();

        if (stream1 != music1.Stream)
        {
            stream1 = music1.Stream;
        }

        if (stream2 != music2.Stream)
        {
            stream2 = music2.Stream;
        }

        double timeRemaining = 0.0;
        switch (channel)
        {
            case MusicManagerChannel.Music1:
                if (!music1.Playing)
                {
                    music1LoopCount--;
                    music1.Play(0);
                }
                musicChannelLerp = Mathf.Clamp(musicChannelLerp - (float)(delta / targetFadeTime), 0, 1);
                timeRemaining = stream1.GetLength() - music1.GetPlaybackPosition();

                if (music1LoopCount <= 0 && timeRemaining < targetFadeTime)
                {
                    music1LoopCount = musicLoopCountMax;
                    channel = MusicManagerChannel.Music2;
                    music2.Stream = songs.GetRandom();
                    music2LoopCount = musicLoopCountMax;
                    music2.Play(0);
                }
                break;
            case MusicManagerChannel.Music2:
                if (!music2.Playing)
                {
                    music2LoopCount--;
                    music2.Play(0);
                }

                musicChannelLerp = Mathf.Clamp(musicChannelLerp + (float)(delta / targetFadeTime), 0, 1);
                timeRemaining = stream2.GetLength() - music2.GetPlaybackPosition();

                if (music2LoopCount <= 0 && timeRemaining < targetFadeTime)
                {
                    music2LoopCount = musicLoopCountMax;
                    channel = MusicManagerChannel.Music1;
                    music1.Stream = songs.GetRandom();
                    music1LoopCount = musicLoopCountMax;
                    music1.Play(0);
                }
                break;
        }

        musicVolume = Options.GetFloat("MusicVolume", 0.5f);
        if (musicVolume <= 1f)
        {
            musicVolumeDb = musicVolumeDbMin;
        }
        else
        {
            musicVolumeDb = Mathf.Lerp(musicVolumeDbLow, musicVolumeDbMax, musicVolume / 100);
        }

        music1.VolumeDb = Mathf.Lerp(musicVolumeDb, -80, musicChannelLerp);
        music2.VolumeDb = Mathf.Lerp(-80, musicVolumeDb, musicChannelLerp);
    }

}
