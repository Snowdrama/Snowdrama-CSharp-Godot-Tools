using Godot;
using System;
using System.Collections.Generic;
public partial class MessagedSoundPool : Node
{
    [Export] int audioPlayerCount = 64;

    [Export] bool use2DPlayers = true;
    [Export] bool use3DPlayers = true;


    List<AudioStreamPlayer> players = new List<AudioStreamPlayer>();
    List<AudioStreamPlayer2D> players2D = new List<AudioStreamPlayer2D>();
    List<AudioStreamPlayer3D> players3D = new List<AudioStreamPlayer3D>();

    PlaySoundMessage playSoundMessage;
    PlayPosiotionedSoundMessage2D playPosiotionedSoundMessage2D;
    PlayPosiotionedSoundMessage3D playPosiotionedSoundMessage3D;
    public override void _EnterTree()
    {
        base._EnterTree();
        for (int i = 0; i < audioPlayerCount; i++)
        {
            var newPlayer = new AudioStreamPlayer();
            newPlayer.Autoplay = false;
            players.Add(newPlayer);
            this.AddChild(newPlayer);
        }
        if (use2DPlayers)
        {
            for (int i = 0; i < audioPlayerCount; i++)
            {
                var newPlayer = new AudioStreamPlayer2D();
                newPlayer.Autoplay = false;
                players2D.Add(newPlayer);
                this.AddChild(newPlayer);
            }
        }
        if (use3DPlayers)
        {
            for (int i = 0; i < audioPlayerCount; i++)
            {
                var newPlayer = new AudioStreamPlayer3D();
                newPlayer.Autoplay = false;
                players3D.Add(newPlayer);
                this.AddChild(newPlayer);
            }
        }
        foreach (var child in this.GetChildren())
        {
            if(child is AudioStreamPlayer2D)
            {
                players2D.Add((AudioStreamPlayer2D)child);    
            }
        }

        playSoundMessage = Messages.Get<PlaySoundMessage>();
        playSoundMessage.AddListener(PlaySound);
        playPosiotionedSoundMessage2D = Messages.Get<PlayPosiotionedSoundMessage2D>();
        playPosiotionedSoundMessage2D.AddListener(PlayPositionedSound2D);
        playPosiotionedSoundMessage3D = Messages.Get<PlayPosiotionedSoundMessage3D>();
        playPosiotionedSoundMessage3D.AddListener(PlayPositionedSound3D);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        playSoundMessage.RemoveListener(PlaySound);
        playPosiotionedSoundMessage2D.RemoveListener(PlayPositionedSound2D);
        playPosiotionedSoundMessage3D.RemoveListener(PlayPositionedSound3D);


        Messages.Return<PlaySoundMessage>();
        Messages.Return<PlayPosiotionedSoundMessage2D>();
        Messages.Return<PlayPosiotionedSoundMessage3D>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="busName"></param>
    /// <param name="volume">Value 0-100 percentage of the bus volume so 100% volume at 50% bus volume is 50% volume</param>
    public void PlaySound(AudioStream stream, string busName, float volume)
    {

        for (int i = 0; i < players2D.Count; i++)
        {
            if (!players2D[i].Playing)
            {
                var volumeDB = Mathf.Lerp(-80, 0, volume / 1);
                players[i].VolumeDb = volumeDB;
                players[i].Bus = busName;
                players[i].Stream = stream;
                players[i].Play();
                return;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="playPosition"></param>
    /// <param name="busName"></param>
    /// <param name="volume">Value 0-100 percentage of the bus volume so 100% volume at 50% bus volume is 50% volume</param>
    public void PlayPositionedSound2D(AudioStream stream, Vector2 playPosition, string busName, float volume)
    {
        if (!use2DPlayers)
        {
            GD.PrintErr("Tried to play a 2D sound but not using 2D sound pool, See MessagedSoundPool");
            return;
        }
        for (int i = 0; i < players2D.Count; i++)
        {
            if (!players2D[i].Playing)
            {
                var busVolumeDb = Mathf.Lerp(-80, 0, volume / 100);
                players2D[i].VolumeDb = busVolumeDb;
                players2D[i].Bus = busName;
                players2D[i].Stream = stream;
                players2D[i].Position = playPosition;
                players2D[i].Play();
                return;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="playPosition"></param>
    /// <param name="busName"></param>
    /// <param name="volume">Value 0-100 percentage of the bus volume so 100% volume at 50% bus volume is 50% volume</param>
    public void PlayPositionedSound3D(AudioStream stream, Vector3 playPosition, string busName, float volume)
    {
        if (!use3DPlayers)
        {
            GD.PrintErr("Tried to play a 3D sound but not using 3D sound pool, See MessagedSoundPool");
            return;
        }

        for (int i = 0; i < players3D.Count; i++)
        {
            if (!players3D[i].Playing)
            {
                var busVolume = Options.GetFloat($"{busName}", 50f);
                var busVolumeDb = Mathf.Lerp(-80, 0, busVolume / 100);
                players3D[i].VolumeDb = busVolumeDb;
                players3D[i].Bus = busName;
                players3D[i].Stream = stream;
                players3D[i].Position = playPosition;
                players3D[i].Play();
                return;
            }
        }
    }
}
