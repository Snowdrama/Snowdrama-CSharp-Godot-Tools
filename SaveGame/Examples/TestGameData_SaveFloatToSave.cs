
using Godot;

public partial class TestGameData_SaveFloatToSave : LineEdit
{
    private float state;
    [Export] private string key;
    public override void _Ready()
    {
        var value = GameData.GetFloat(key, 0.0f);
        Debug.Log($"Loaded {key} from GameData, Value: {value}");
        this.SetBlockSignals(true);
        this.Text = $"{value}";
        this.SetBlockSignals(false);
        this.TextChanged += OnChanged;
    }

    public void OnChanged(string newState)
    {
        float value;
        if (float.TryParse(newState, out value))
        {
            GameData.SetFloat(key, value);
        }
    }
}
