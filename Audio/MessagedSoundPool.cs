using Godot;
using Snowdrama.Core;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MessagedSoundPool : Node
{
    [Export] int audioPlayerCount = 64;

    [Export] bool use2DPlayers = true;
    [Export] bool use3DPlayers = true;

    Stack<AudioStreamPlayer> players = new Stack<AudioStreamPlayer>();
    Stack<AudioStreamPlayer2D> players2D = new Stack<AudioStreamPlayer2D>();
    Stack<AudioStreamPlayer3D> players3D = new Stack<AudioStreamPlayer3D>();

    List<AudioStreamPlayer> usedPlayers = new List<AudioStreamPlayer>();
    List<AudioStreamPlayer2D> usedPlayers2D = new List<AudioStreamPlayer2D>();
    List<AudioStreamPlayer3D> usedPlayers3D = new List<AudioStreamPlayer3D>();

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
            newPlayer.Finished += () =>
            {
                if (!players.Contains(newPlayer))
                {
                    players.Push(newPlayer);
                }
            };
            players.Push(newPlayer);
            this.AddChild(newPlayer);
        }
        if (use2DPlayers)
        {
            for (int i = 0; i < audioPlayerCount; i++)
            {
                var newPlayer = new AudioStreamPlayer2D();
                newPlayer.Autoplay = false;
                newPlayer.Finished += () =>
                {
                    if (!players2D.Contains(newPlayer))
                    {
                        players2D.Push(newPlayer);
                    }
                };
                players2D.Push(newPlayer);
                this.AddChild(newPlayer);
            }
        }
        if (use3DPlayers)
        {
            for (int i = 0; i < audioPlayerCount; i++)
            {
                var newPlayer = new AudioStreamPlayer3D();
                newPlayer.Autoplay = false;
                newPlayer.Finished += () =>
                {
                    if (!players3D.Contains(newPlayer))
                    {
                        players3D.Push(newPlayer);
                    }
                };
                players3D.Push(newPlayer);
                this.AddChild(newPlayer);
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

    public override void _Process(double delta)
    {
        base._Process(delta);
        DebugDraw2D.SetText("Players count:", $"{players.Count}");
        DebugDraw2D.SetText("Players2D count:", $"{players2D.Count}");
        DebugDraw2D.SetText("Players3D count:", $"{players3D.Count}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="busName"></param>
    /// <param name="volume">Value 0-100 percentage of the bus volume so 100% volume at 50% bus volume is 50% volume</param>
    public void PlaySound(AudioStream stream, string busName, float volume, Vector2 pitchRange)
    {
        if(stream == null)
        {
            Debug.LogError($"Tried to play a sound with a null stream!");
            return;
        }
        if (players.Count > 0)
        {
            var player = players.Pop();
            var volumeDB = Mathf.Lerp(-80, 0, volume / 1.0f);
            player.VolumeDb = volumeDB;
            player.Bus = busName;
            player.Stream = stream;
            player.PitchScale = RandomAndNoise.RandomRange(pitchRange.X, pitchRange.Y);
            player.Play();

            usedPlayers.Add(player);
        }
        else
        {
            Debug.LogError("Message pool doesn't have any available sounds!");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="playPosition"></param>
    /// <param name="busName"></param>
    /// <param name="volume">Value 0-100 percentage of the bus volume so 100% volume at 50% bus volume is 50% volume</param>
    public void PlayPositionedSound2D(AudioStream stream, Vector2 playPosition, string busName, float volume, Vector2 pitchRange)
    {
        if (stream == null)
        {
            Debug.LogError($"Tried to play a sound with a null stream!");
            return;
        }
        if (!use2DPlayers)
        {
            Debug.LogError("Tried to play a 2D sound but not using 2D sound pool, See MessagedSoundPool");
            return;
        }

        if (players2D.Count > 0)
        {
            var player = players2D.Pop();
            var volumeDB = Mathf.Lerp(-80, 0, volume / 1);
            player.VolumeDb = volumeDB;
            player.Bus = busName;
            player.Stream = stream;
            player.Position = playPosition;
            player.PitchScale = RandomAndNoise.RandomRange(pitchRange.X, pitchRange.Y);
            player.Play();
            usedPlayers2D.Add(player);
        }
        else
        {
            Debug.LogError("2D Message pool doesn't have any available sounds!");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="playPosition"></param>
    /// <param name="busName"></param>
    /// <param name="volume">Value 0-100 percentage of the bus volume so 100% volume at 50% bus volume is 50% volume</param>
    public void PlayPositionedSound3D(AudioStream stream, Vector3 playPosition, string busName, float volume, Vector2 pitchRange)
    {
        if (stream == null)
        {
            Debug.LogError($"Tried to play a sound with a null stream!");
            return;
        }
        if (!use3DPlayers)
        {
            Debug.LogError("Tried to play a 3D sound but not using 3D sound pool, See MessagedSoundPool");
            return;
        }
        if (players3D.Count > 0)
        {
            var player = players3D.Pop();
            var volumeDB = Mathf.Lerp(-80, 0, volume / 1);
            player.VolumeDb = volumeDB;
            player.Bus = busName;
            player.Stream = stream;
            player.Position = playPosition;
            player.PitchScale = RandomAndNoise.RandomRange(pitchRange.X, pitchRange.Y);
            player.Play();
            usedPlayers3D.Add(player);
        }
        else
        {
            Debug.LogError("3D Message pool doesn't have any available sounds!");
        }
    }
}
