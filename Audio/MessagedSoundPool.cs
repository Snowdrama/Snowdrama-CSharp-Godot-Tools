using Godot;
using System;
using System.Collections.Generic;
public partial class MessagedSoundPool : Node
{
    [Export] int audioPlayerCount = 64;
    List<AudioStreamPlayer2D> players = new List<AudioStreamPlayer2D>();

    PlaySoundMessage playSoundMessage;
    PlayPosiotionedSoundMessage playPosiotionedSoundMessage;
    public override void _EnterTree()
    {
        base._EnterTree();
        for (int i = 0; i < audioPlayerCount; i++)
        {
            var newPlayer = new AudioStreamPlayer2D();
            newPlayer.Autoplay = false;
            players.Add(newPlayer);
            this.AddChild(newPlayer);
        }
        foreach(var child in this.GetChildren())
        {
            if(child is AudioStreamPlayer2D)
            {
                players.Add((AudioStreamPlayer2D)child);    
            }
        }

        playSoundMessage = Messages.Get<PlaySoundMessage>();
        playSoundMessage.AddListener(PlaySound);
        playPosiotionedSoundMessage = Messages.Get<PlayPosiotionedSoundMessage>();
        playPosiotionedSoundMessage.AddListener(PlayPositionedSound);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        playSoundMessage.RemoveListener(PlaySound);
        playPosiotionedSoundMessage.RemoveListener(PlayPositionedSound);
    }
    public void PlaySound(AudioStream stream, string busName)
    {

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].Playing)
            {
                var busVolume = Options.GetFloat($"{busName}", 50f);
                var busVolumeDb = Mathf.Lerp(-80, 0, busVolume / 100);
                GD.Print($"Playing sound to bus {busName} at {busVolume} / 100 = ");
                players[i].VolumeDb = busVolumeDb;

                players[i].Bus = busName;
                players[i].Stream = stream;
                players[i].Attenuation = 0.00f; //no falloff
                players[i].MaxDistance = 10000; //large GRID_SIZE
                players[i].Play();
                return;
            }
        }
    }
    public void PlayPositionedSound(AudioStream stream, Vector2 playPosition, string busName)
	{

		for (int i = 0; i < players.Count; i++)
		{
			if (!players[i].Playing)
            {
                var busVolume = Options.GetFloat($"{busName}", 50f);
                var busVolumeDb = Mathf.Lerp(-80, 0, busVolume / 100);
                players[i].VolumeDb = busVolumeDb;
                players[i].Bus = busName;
                players[i].Stream = stream;
				players[i].Position = playPosition;
                players[i].Play();
				return;
			}
        }
	}
}
