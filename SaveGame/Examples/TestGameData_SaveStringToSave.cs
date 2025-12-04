
using Godot;

public partial class TestGameData_SaveStringToSave : LineEdit
{
    private string state;
    [Export] private string key;
    public override void _Ready()
    {
        var value = GameData.GetString(key, "N/A");
        Debug.Log($"Loaded Srring {key} from GameData, Value: {value}");
        this.SetBlockSignals(true);
        this.Text = value;
        this.SetBlockSignals(false);
        this.TextChanged += OnChanged;
    }

    public void OnChanged(string newState)
    {
        GameData.SetString(key, newState);
    }
}
