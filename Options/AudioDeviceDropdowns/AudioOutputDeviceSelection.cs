using Godot;

public partial class AudioOutputDeviceSelection : OptionButton
{
    public override void _Ready()
    {
        this.VisibilityChanged += AudioDeviceSelection_VisibilityChanged;
        this.ItemSelected += AudioDeviceSelection_ItemSelected;

        var outputDeviceLoad = Options.GetString("OutputDevice", "Default");
        Debug.Log($"Output Device Loaded: {outputDeviceLoad}");
        AudioServer.OutputDevice = outputDeviceLoad;
        UpdateDeviceList();
        UpdateOptionToValue(outputDeviceLoad);
    }

    private void AudioDeviceSelection_ItemSelected(long index)
    {
        var device = AudioServer.GetOutputDeviceList()[index];
        Debug.Log($"Output Device Saved: {device}");
        AudioServer.OutputDevice = device;
        Options.SetString("OutputDevice", device);
    }

    private void AudioDeviceSelection_VisibilityChanged()
    {
        if (!this.Visible) { return; }
        UpdateDeviceList();
        UpdateOptionToValue(AudioServer.OutputDevice);
    }

    public void UpdateDeviceList()
    {
        this.Clear();
        var outputDeviceList = AudioServer.GetOutputDeviceList();
        foreach (var item in outputDeviceList)
        {
            this.AddItem(item);
        }
    }

    public void UpdateOptionToValue(string deviceName)
    {
        var outputDeviceList = AudioServer.GetOutputDeviceList();
        Debug.Log($"Changing Output Device Option To: {deviceName}");
        for (int i = 0; i < outputDeviceList.Length; i++)
        {
            if (outputDeviceList[i] == deviceName)
            {
                this.Select(i);
            }
        }
    }
}
