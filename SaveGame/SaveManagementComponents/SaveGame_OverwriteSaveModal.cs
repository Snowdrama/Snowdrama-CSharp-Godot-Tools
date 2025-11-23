
using Godot;

public partial class SaveGame_OverwriteSaveModalMessage : AMessage<int, string> { }
public partial class SaveGame_OverwriteSaveModal : Node
{
    [Export] private ModalPanel SaveGamePanel;
    [Export] private LineEdit SaveName;
    [Export] private Button SaveButton;
    [Export] private Button CancelButton;

    private int saveSlot = 0;
    private SaveGame_OverwriteSaveModalMessage openSavegameModal;
    private void OnEnable()
    {
        SaveButton.Pressed += SaveToExistingSlot;
        CancelButton.Pressed += CancelSave;

        openSavegameModal = Messages.Get<SaveGame_OverwriteSaveModalMessage>();
        openSavegameModal.AddListener(OpenSaveModal);
    }
    private void OnDisable()
    {
        SaveButton.Pressed -= SaveToExistingSlot;
        CancelButton.Pressed -= CancelSave;

        openSavegameModal.RemoveListener(OpenSaveModal);
        openSavegameModal = null;
        Messages.Return<SaveGame_OverwriteSaveModalMessage>();
    }

    private void OpenSaveModal(int saveSlot, string existingName)
    {
        this.saveSlot = saveSlot;
        if (!string.IsNullOrEmpty(existingName))
        {
            SaveName.Text = existingName;
        }

        SaveGamePanel.SetActive(true);
    }

    private void SaveToExistingSlot()
    {
        if (!SaveManager.SaveGame(saveSlot, GameData.GetGameData(), false, SaveName.Text))
        {
            Messages.GetOnce<OpenConfirmationModalMessage>().Dispatch(
                "Are you sure you want to override the save?",
                new ModalButtonData()
                {
                    Text = "Yes",
                    PressCallback = ForceSave,
                    DisableTime = 2.0f,
                },
                new ModalButtonData()
                {
                    Text = "No",
                    PressCallback = CancelSave,
                    DisableTime = 0.0f,
                }
            );
        }
    }

    public void ForceSave()
    {
        SaveManager.SaveGame(saveSlot, GameData.GetGameData(), true, SaveName.Text);
        SaveGamePanel.SetActive(false);
    }
    public void CancelSave()
    {
        //do nothing XD
        SaveGamePanel.SetActive(false);
    }
}
