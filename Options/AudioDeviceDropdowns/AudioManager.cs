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

            AudioServer.SetBusVolumeDb(index, Mathf.Lerp(-80, 10, lerp));
        }
    }


    public static void SetVolume(string volumeKey, float volumeValue, float volumeMinValue = 0, float volumeMaxValue = 1)
    {
        var index = AudioServer.GetBusIndex(volumeKey);
        if (index != -1)
        {
            var lerp = Mathf.InverseLerp(volumeMinValue, volumeMaxValue, volumeValue);
            AudioServer.SetBusVolumeDb(index, Mathf.Lerp(-80, 10, lerp));
        }
    }
}
