using System.Collections.Generic;
using System.Linq;
using Godot;


public partial class SaveGameListDisplay : Node
{
    [Export] private bool AddSaves = true;
    [Export] private bool AddAutosaves = false;
    [Export] private PackedScene saveButtonPrefab;
    [Export] private PackedScene autoSaveButtonPrefab;
    [Export] private Control buttonContainer;
    [Export] private float buttonHeight = 25.0f;
    private SaveGameListChanged savesChanged;
    private void OnEnable()
    {
        savesChanged = Messages.Get<SaveGameListChanged>();
        savesChanged.AddListener(LoadSaveList);

    }

    private void OnDisable()
    {
        savesChanged.RemoveListener(LoadSaveList);
        savesChanged = null;
        Messages.Return<SaveGameListChanged>();
    }

    private List<Node> buttons = new List<Node>();

    public override void _Ready()
    {
        LoadSaveList();
    }

    public override void _Process(double delta)
    {
    }
    private void LoadSaveList()
    {

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].QueueFree();
        }
        buttons.Clear();

        var saveDataStruct = SaveManager.GetSaveList();
        int saveCount = 0;

        if (AddSaves)
        {
            saveCount += saveDataStruct.saveLocations.Count;
        }

        if (AddAutosaves)
        {
            saveCount += saveDataStruct.autoSaveLocations.Count;
        }

        //buttonContainer.offsetMin = new Vector2(0, 0);
        //buttonContainer.offsetMax = new Vector2(0, buttonHeight * saveCount);


        foreach (var kvp in saveDataStruct.autoSaveLocations.OrderByDescending(x => x.Value.dateModified))
        {
            var go = autoSaveButtonPrefab.Instantiate();
            buttonContainer.AddChild(go);

            if (go is ISaveButton isb)
            {
                isb.SetButtonInfo(kvp.Key, $"{kvp.Value.name} - {kvp.Value.dateModified}");
            }
            buttons.Add(go);
        }


        foreach (var kvp in saveDataStruct.saveLocations.OrderByDescending(x => x.Value.dateModified))
        {
            var go = autoSaveButtonPrefab.Instantiate();
            buttonContainer.AddChild(go);
            if (go is ISaveButton isb)
            {
                isb.SetButtonInfo(kvp.Key, $"{kvp.Value.name} - {kvp.Value.dateModified}");
            }
            buttons.Add(go);
        }

        //this.verticalNormalizedPosition = 1;
    }
}


public interface ISaveButton
{
    void SetButtonInfo(int saveSlot, string saveName);
}