
using Godot;


public partial class SaveGame_OverwriteSaveButton : Node, ISaveButton
{
    private int saveSlot;
    private string saveName;

    [Export] private Button saveButton;
    [Export] private Button deleteButton;
    [Export] private Label saveButtonText;

    public void SetSaveInfo(int saveSlot, string saveName)
    {
        this.saveName = saveName;
        this.saveSlot = saveSlot;
        saveButtonText.Text = saveName;
    }
    public override void _Ready()
    {
        saveButton.Pressed += OpenModal;
        deleteButton.Pressed += DeleteSave;
    }

    private void OpenModal()
    {
        Messages.GetOnce<SaveGame_OverwriteSaveModalMessage>().Dispatch(saveSlot, saveName);
    }
    private void DeleteSave()
    {
        Messages.GetOnce<OpenConfirmationModalMessage>().Dispatch(
            $"Are you sure you want to delete the save?\n {saveName}",
            new ModalButtonData()
            {
                Text = "Yes",
                PressCallback = ForceDelete,
                DisableTime = 2.0f,
            },
            new ModalButtonData()
            {
                Text = "No",
                PressCallback = null,
                DisableTime = 0.0f,
            }
        );
    }

    private void ForceDelete()
    {
        SaveManager.DeleteSaveGame(saveSlot, true);
        this.QueueFree();
    }

    public void SetButtonInfo(int saveSlot, string saveName)
    {
        this.saveSlot = saveSlot;
        this.saveName = saveName;
        saveButtonText.Text = saveName;
    }
}
