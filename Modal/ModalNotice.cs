using Godot;
/// <summary>
/// Notice Modals are for showing information
/// to the player but don't have a choice
/// 
/// Examples: 
/// "Save in slot 4 doesn't exist -> Ok"
/// "Can't delete last object -> Ok"
/// "Needs a key to open -> Ok"
/// </summary>
/// 


public partial class ModalNotice : Node
{
    [Export] private ModalPanel modalPanel;
    [Export] private Label modalText;
    [Export] private Button okButton;

    private OpenNoticeModalMessage saveModalMessage;
    private ModalButtonData ok;

    private void OnEnable()
    {
        saveModalMessage = Messages.Get<OpenNoticeModalMessage>();
        saveModalMessage.AddListener(OpenModal);
    }

    private void OnDisable()
    {
        saveModalMessage.RemoveListener(OpenModal);
        saveModalMessage = null;
        Messages.Return<OpenNoticeModalMessage>();
    }

    public void OpenModal(string setModalText, ModalButtonData okModalData)
    {

        modalText.Text = setModalText;
        ok = okModalData;

        if (ok.DisableTime > 0)
        {
            okButton.Disabled = true;
            okButton.Text = $"{ok.Text} ({ok.DisableTime:F1})";
        }
        else
        {
            okButton.Disabled = false;
            okButton.Text = $"{ok.Text}";
        }
        modalPanel.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void _Ready()
    {
        modalPanel.SetActive(false);
        okButton.Pressed += OkPressed;
    }

    private void CancelPressed()
    {
        modalPanel.SetActive(false);
    }

    private void OkPressed()
    {
        ok.PressCallback?.Invoke();
        modalPanel.SetActive(false);
    }

    // Update is called once per frame
    public override void _Process(double delta)
    {
        if (ok.DisableTime > 0)
        {
            ok.DisableTime -= (float)delta;
            okButton.Disabled = true;
            okButton.Text = $"{ok.Text} ({ok.DisableTime:F1})";
            if (ok.DisableTime <= 0)
            {
                okButton.Disabled = false;
                okButton.Text = $"{ok.Text}";
            }
        }
    }
}
