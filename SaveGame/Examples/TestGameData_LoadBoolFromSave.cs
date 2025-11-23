using Godot;

public partial class TestGameData_LoadBoolFromSave : Label
{
    [Export] private string key;
    public override void _Ready()
    {
        this.Text = $"FloatData['{key}']: {GameData.GetBool(key, false)}";
    }
}
