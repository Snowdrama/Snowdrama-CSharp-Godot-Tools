using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;


public partial class AudioManager : Node
{
    [Export]
    public Godot.Collections.Array<string> VolumeKeys = new Godot.Collections.Array<string>()
    {
        Options.MASTER_VOLUME_OPTION_KEY,
        Options.MUSIC_VOLUME_OPTION_KEY,
        Options.SOUND_VOLUME_OPTION_KEY,
        Options.VOICE_VOLUME_OPTION_KEY,
    };
    [Export]
    public Godot.Collections.Array<float> DefaultVolume = new Godot.Collections.Array<float>()
    {
        1.0f,
        0.8f,
        0.8f,
        0.8f,
    };

    public override void _Ready()
    {
        for (int i = 0; i < VolumeKeys.Count; i++)
        {
            var lerp = Mathf.Clamp(Options.GetFloat(VolumeKeys[i], DefaultVolume[i]), 0.0f, 1.0f);
            var index = AudioServer.GetBusIndex(VolumeKeys[i]);
            if (index >= 0)
            {
                AudioServer.SetBusVolumeDb(index, Mathf.Lerp(-80, 0, lerp));
            }
            else
            {
                GD.PrintErr($"The key {VolumeKeys[i]} isn't listed as a bus. Please check the audio options to ensure only the keys listen in the audio bus resource");
            }
        }
    }


    public static void SetVolume(string volumeKey, float volumeValue, float volumeMinValue = 0, float volumeMaxValue = 1)
    {
        var index = AudioServer.GetBusIndex(volumeKey);
        if (index >= 0)
        {
            var lerp = Mathf.InverseLerp(volumeMinValue, volumeMaxValue, volumeValue);

            
            AudioServer.SetBusVolumeDb(index, Mathf.Lerp(-80, 0, lerp));
        }
        else
        {
            GD.PrintErr($"The key {volumeKey} isn't listed as a bus. Please check the audio options to ensure only the keys listen in the audio bus resource");
        }
    }
}
