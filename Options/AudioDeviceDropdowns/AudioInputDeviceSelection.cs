using Godot;

public partial class AudioInputDeviceSelection : OptionButton
{
    public override void _Ready()
    {
        this.VisibilityChanged += AudioDeviceSelection_VisibilityChanged;
        this.ItemSelected += AudioDeviceSelection_ItemSelected;

        var inputDeviceLoad = Options.GetString("InputDevice", "Default");
        Debug.Log($"Input Device Loaded: {inputDeviceLoad}");
        AudioServer.InputDevice = inputDeviceLoad;
        UpdateDeviceList();
        UpdateOptionToValue(inputDeviceLoad);
    }

    private void AudioDeviceSelection_ItemSelected(long index)
    {
        var device = AudioServer.GetInputDeviceList()[index];
        Debug.Log($"Input Device Saved: {device}");
        AudioServer.InputDevice = device;
        Options.SetString("InputDevice", device);
    }

    private void AudioDeviceSelection_VisibilityChanged()
    {
        if (!this.Visible) { return; }
        UpdateDeviceList();
        UpdateOptionToValue(AudioServer.InputDevice);
    }

    public void UpdateDeviceList()
    {
        this.Clear();
        var inputDeviceList = AudioServer.GetInputDeviceList();
        foreach (var item in inputDeviceList)
        {
            this.AddItem(item);
        }
    }

    public void UpdateOptionToValue(string deviceName)
    {
        var inputDeviceList = AudioServer.GetInputDeviceList();
        Debug.Log($"Changing Input Device Option To: {deviceName}");
        for (int i = 0; i < inputDeviceList.Length; i++)
        {
            if (inputDeviceList[i] == deviceName)
            {
                this.Select(i);
            }
        }
    }
}
