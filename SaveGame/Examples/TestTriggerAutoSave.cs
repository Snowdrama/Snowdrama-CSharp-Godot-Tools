using Godot;

public partial class TestTriggerAutoSave : Node
{
    [Export] private bool autoSave = true;
    [Export] private float autoSaveTime_Max = 60.0f * 60.0f * 15.0f;//fifteen minutes
    [Export] private float autoSaveTime = 60.0f * 60.0f * 15.0f;//fifteen minutes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void _Ready()
    {

    }

    // Update is called once per frame
    public override void _Process(double delta)
    {
        if (!autoSave)
        {
            return;
        }

        autoSaveTime -= (float)delta;

        if (autoSaveTime <= 0)
        {
            Debug.Log("Triggering Auto Save!");
            SaveManager.AutoSave(GameData.GetGameData());
            autoSaveTime = autoSaveTime_Max;
        }
    }
}
