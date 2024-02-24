using Godot;
using System;

public partial class AudioOutputDeviceSelection : OptionButton
{
    [Export] Options options;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.VisibilityChanged += AudioDeviceSelection_VisibilityChanged;
        this.ItemSelected += AudioDeviceSelection_ItemSelected;

        var outputDeviceLoad = Options.GetString("OutputDevice", "Default");
        GD.Print($"Output Device Loaded: {outputDeviceLoad}");
        AudioServer.OutputDevice = outputDeviceLoad;
        UpdateDeviceList();
        UpdateOptionToValue(outputDeviceLoad);
    }

    private void AudioDeviceSelection_ItemSelected(long index)
    {
        var device = AudioServer.GetOutputDeviceList()[index];
        GD.Print($"Output Device Saved: {device}");
        AudioServer.OutputDevice = device;
        Options.SetString("OutputDevice", device);
    }

    private void AudioDeviceSelection_VisibilityChanged()
    {
        if (!this.Visible) { return; }
        UpdateDeviceList();
        UpdateOptionToValue(AudioServer.OutputDevice);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
        GD.Print($"Changing Output Device Option To: {deviceName}");
        for (int i = 0; i < outputDeviceList.Length; i++)
        {
            if (outputDeviceList[i] == deviceName)
            {
                this.Select(i);
            }
        }
    }
}
