using Godot;


public partial class SaveGame_CreateNewSaveButton : Button
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void _Ready()
    {
        this.Pressed += OpenModal;
    }

    private void OpenModal()
    {
        Messages.GetOnce<SaveGame_CreateNewSaveModalMessage>().Dispatch();
    }
}
