using Godot;
using System;

public partial class InputDeviceScript : OptionButton
{
    public override void _Ready()
    {
        this.VisibilityChanged += AudioDeviceSelection_VisibilityChanged;
        this.ItemSelected += AudioDeviceSelection_ItemSelected;

        UpdateDeviceList();
    }

    private void AudioDeviceSelection_ItemSelected(long index)
    {
    }

    private void AudioDeviceSelection_VisibilityChanged()
    {
        UpdateDeviceList();
    }

    public void UpdateDeviceList()
    {
        this.Clear();
        var connectedJuoypadIds = Input.GetConnectedJoypads();
        for (int i = 0; i < connectedJuoypadIds.Count; i++)
        {
            var name = Input.GetJoyName(connectedJuoypadIds[i]);
            this.AddItem(name);
        }
    }

    public void UpdateOptionToValue(string deviceName)
    {
    }
}
