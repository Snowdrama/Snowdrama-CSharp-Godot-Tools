using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;


public partial class AudioManager : Node
{
    [Export] public Godot.Collections.Array<string> VolumeKeys;
    public override void _Ready()
    {
        for (int i = 0; i < VolumeKeys.Count; i++)
        {
            var lerp = Mathf.Clamp(Options.GetFloat(VolumeKeys[i], 0.5f), 0.0f, 1.0f);
            var index = AudioServer.GetBusIndex(VolumeKeys[i]);
            AudioServer.SetBusVolumeDb(index, Mathf.Lerp(-80, 10, lerp));
            //GD.PrintErr($"Bus: {VolumeKeys[i]} Volume {Mathf.Lerp(-80, 10, lerp)}");
        }
    }


    public static void SetVolume(string volumeKey, float volumeValue, float volumeMinValue = 0, float volumeMaxValue = 1)
    {
        var index = AudioServer.GetBusIndex(volumeKey);
        if (index != -1)
        {
            var lerp = Mathf.InverseLerp(volumeMinValue, volumeMaxValue, volumeValue);
            AudioServer.SetBusVolumeDb(index, Mathf.Lerp(-80, 10, lerp));
            //GD.PrintErr($"Bus: {volumeKey} Volume {Mathf.Lerp(-80, 10, lerp)}");
        }
    }
}
