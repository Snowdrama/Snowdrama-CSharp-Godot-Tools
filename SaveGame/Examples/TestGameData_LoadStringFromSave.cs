
using Godot;

public partial class TestGameData_LoadStringFromSave : Label
{
    [Export] private string key;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void _Ready()
    {
        this.Text = $"FloatData['{key}']: {GameData.GetString(key, "N/A")}";
    }
}
