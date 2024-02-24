using Godot;
using System;

public partial class AudioInputDeviceSelection : OptionButton
{
    [Export] Options options;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.VisibilityChanged += AudioDeviceSelection_VisibilityChanged;
        this.ItemSelected += AudioDeviceSelection_ItemSelected;

        var inputDeviceLoad = Options.GetString("InputDevice", "Default");
        GD.Print($"Input Device Loaded: {inputDeviceLoad}");
        AudioServer.InputDevice = inputDeviceLoad;
        UpdateDeviceList();
        UpdateOptionToValue(inputDeviceLoad);
    }

    private void AudioDeviceSelection_ItemSelected(long index)
    {
        var device = AudioServer.GetInputDeviceList()[index];
        GD.Print($"Input Device Saved: {device}");
        AudioServer.InputDevice = device;
        Options.SetString("InputDevice", device);
    }

    private void AudioDeviceSelection_VisibilityChanged()
    {
        if (!this.Visible) { return; }
        UpdateDeviceList();
        UpdateOptionToValue(AudioServer.InputDevice);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
        GD.Print($"Changing Input Device Option To: {deviceName}");
        for (int i = 0; i < inputDeviceList.Length; i++)
        {
            if (inputDeviceList[i] == deviceName)
            {
                this.Select(i);
            }
        }
    }
}
