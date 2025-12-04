using Godot;


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
    public Godot.Collections.Array<double> DefaultVolume = new Godot.Collections.Array<double>()
    {
        1.0,
        0.8f,
        0.8f,
        0.8f,
    };

    public override void _Ready()
    {
        for (int i = 0; i < VolumeKeys.Count; i++)
        {
            var lerp = Mathf.Clamp(Options.GetDouble(VolumeKeys[i], DefaultVolume[i]), 0.0, 1.0);
            var index = AudioServer.GetBusIndex(VolumeKeys[i]);
            Debug.Log($"Loading VolumeKeys[{i}]: {VolumeKeys[i]} => {lerp}");
            if (index >= 0)
            {
                AudioServer.SetBusVolumeDb(index, (float)Mathf.LinearToDb(lerp));
                Options.SetDouble(VolumeKeys[i], lerp);
            }
            else
            {
                Debug.LogError($"The key {VolumeKeys[i]} isn't listed as a bus. Please check the audio options to ensure only the keys listen in the audio bus resource");
            }
        }
    }


    public static void SetVolume(string volumeKey, double db, float volumeMinValue = 0, float volumeMaxValue = 1)
    {
        var index = AudioServer.GetBusIndex(volumeKey);
        if (index >= 0)
        {
            AudioServer.SetBusVolumeDb(index, (float)db);
        }
        else
        {
            Debug.LogError($"The key {volumeKey} isn't listed as a bus. Please check the audio options to ensure only the keys listen in the audio bus resource");
        }
    }

    public static float GetVolume(string volumeKey, float volumeMinValue = 0, float volumeMaxValue = 1)
    {
        var index = AudioServer.GetBusIndex(volumeKey);
        if (index >= 0)
        {
            return AudioServer.GetBusVolumeDb(index);
        }
        else
        {
            GD.PrintErr($"The key {volumeKey} isn't listed as a bus. Please check the audio options to ensure only the keys listen in the audio bus resource");
        }
        return 0;
    }
}
