
using Godot;

public partial class SaveGame_CreateNewSaveModalMessage : AMessage { }
public partial class SaveGame_CreateNewSaveModal : Node
{
    [Export] private ModalPanel SaveGamePanel;
    [Export] private LineEdit SaveName;
    [Export] private Button SaveButton;
    [Export] private Button CancelButton;

    private SaveGame_CreateNewSaveModalMessage openSavegameModal;
    private void OnEnable()
    {
        SaveButton.Pressed += SaveToNewSlot;
        CancelButton.Pressed += Cancel;
        openSavegameModal = Messages.Get<SaveGame_CreateNewSaveModalMessage>();
        openSavegameModal.AddListener(OpenSaveModal);
    }
    private void OnDisable()
    {
        SaveButton.Pressed -= SaveToNewSlot;
        CancelButton.Pressed -= Cancel;
        openSavegameModal.RemoveListener(OpenSaveModal);
        openSavegameModal = null;
        Messages.Return<SaveGame_CreateNewSaveModalMessage>();
    }

    private void OpenSaveModal()
    {
        SaveName.Text = "";
        SaveGamePanel.SetActive(true);
    }

    private void SaveToNewSlot()
    {
        if (SaveManager.SaveGameToNewSlot(GameData.GetGameData(), true, SaveName.Text))
        {
            Messages.GetOnce<OpenNoticeModalMessage>().Dispatch(
                $"Game Saved!",
                new ModalButtonData()
                {
                    Text = "Ok",
                    PressCallback = Complete,
                    DisableTime = 2.0f,
                }
            );
        }
        else
        {
            Messages.GetOnce<OpenNoticeModalMessage>().Dispatch(
                $"Error: Game was unable to be saved.",
                new ModalButtonData()
                {
                    Text = "Ok",
                    PressCallback = Cancel,
                    DisableTime = 2.0f,
                }
            );
        }
    }

    private void Complete()
    {
        SaveGamePanel.SetActive(false);
    }

    private void Cancel()
    {
        SaveGamePanel.SetActive(false);
    }
}
