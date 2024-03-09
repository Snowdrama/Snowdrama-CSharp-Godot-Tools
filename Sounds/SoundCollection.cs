using Godot;
using System;

[GlobalClass]
public partial class SoundCollection : Resource
{
	[Export(PropertyHint.ArrayType, "AudioStream")] Godot.Collections.Array<AudioStream> sounds = new Godot.Collections.Array<AudioStream>();


	public AudioStream GetRandomSound()
	{
		return sounds.GetRandom();
	}
}
