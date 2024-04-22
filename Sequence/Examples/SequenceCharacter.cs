using Godot;
using Godot.Collections;
using System;
using System.Security;

/// <summary>
/// The idea for this is that the SequenceCharacter accepts messages from sequence steps
/// to change an element in a particular way
/// so rather than the step manipulating the sprite directly it might say... change the "emote"
/// or ask to animate to a spot.
/// 
/// Specifically this is for 2D VN style characters
/// where you would swap out the emote entirely.
/// 
/// </summary>
public partial class SequenceCharacter : Control
{
	Godot.Collections.Array<TextureRect> emotes;
	Dictionary<string, TextureRect> namedEmotes;
	TextureRect activeEmote = null;
	public override void _Ready()
	{
		for (int i = 0; i < emotes.Count; i++)
		{
			namedEmotes.Add(emotes[i].Name, emotes[i]);
		}
	}

	public override void _Process(double delta)
	{
	}


	public void ShowEmote(string emoteName)
	{
		if (namedEmotes.ContainsKey(emoteName))
		{
			if(activeEmote != null)
			{
				activeEmote.Hide();
			}
			activeEmote = namedEmotes[emoteName];
			activeEmote.Show();
		}
	}

	public void ShowEmote()
	{

	}
}
