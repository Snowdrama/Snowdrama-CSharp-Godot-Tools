using Godot;
using System;
using System.Collections.Generic;
public partial class MessagedSoundPool : Node
{
    List<AudioStreamPlayer2D> players = new List<AudioStreamPlayer2D>();

    PlaySoundMessage playSoundMessage;
    PlayPosiotionedSoundMessage playPosiotionedSoundMessage;
    public override void _EnterTree()
    {
        base._EnterTree();

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
    public void PlaySound(AudioStream stream)
    {

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].Playing)
            {
                players[i].Stream = stream;
                players[i].Attenuation = 0.00f; //no falloff
                players[i].MaxDistance = 10000; //large Size
                players[i].Play();
                return;
            }
        }
    }
    public void PlayPositionedSound(AudioStream stream, Vector2 playPosition)
	{

		for (int i = 0; i < players.Count; i++)
		{
			if (!players[i].Playing)
			{
				players[i].Stream = stream;
				players[i].Position = playPosition;
                players[i].Play();
				return;
			}
        }
	}
}
