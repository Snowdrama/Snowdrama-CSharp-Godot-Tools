using Godot;


public partial class TestGameData_SaveBoolToSave : CheckButton
{
    [Export] private string key;
    public override void _Ready()
    {
        var value = GameData.GetBool(key, false);
        Debug.Log($"Loaded {key} from GameData, Value: {value}");
        this.SetBlockSignals(true);
        this.ToggleMode = value;
        this.SetBlockSignals(false);
        this.Toggled += OnChanged;
    }

    public void OnChanged(bool newState)
    {
        GameData.SetBool(key, newState);
    }
}
