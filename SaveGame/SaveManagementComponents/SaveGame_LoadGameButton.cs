
using Godot;


public partial class SaveGame_LoadGameButton : Node, ISaveButton
{
    private int saveSlot;
    private string saveName;

    [Export] private Button loadButton;
    [Export] private Button deleteButton;
    [Export] private Label loadButtonText;
    public override void _Ready()
    {
        loadButton.Pressed += TryLoad;
        deleteButton.Pressed += DeleteSave;
    }

    public void TryLoad()
    {
        if (SaveManager.CanLoadSave(saveSlot))
        {
            Messages.GetOnce<OpenConfirmationModalMessage>().Dispatch(
                $"Would you like to load:\n{saveName}",
                new ModalButtonData()
                {
                    Text = "Yes",
                    PressCallback = LoadGame,
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
        else
        {
            Messages.GetOnce<OpenNoticeModalMessage>().Dispatch(
                $"Can't load, save doesn't exist",
                new ModalButtonData()
                {
                    Text = "Ok",
                    PressCallback = null,
                    DisableTime = 2.0f,
                }
            );
        }
    }

    private void LoadGame()
    {
        SaveManager.LoadSave(saveSlot);
    }


    private void DeleteSave()
    {
        Messages.GetOnce<OpenConfirmationModalMessage>().Dispatch(
            $"Are you sure you want to delete the save?\n{saveName}",
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
        loadButtonText.Text = saveName;
    }
}
