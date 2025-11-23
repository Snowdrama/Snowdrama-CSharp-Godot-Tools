using System;
using Godot;

public class OpenConfirmationModalMessage : AMessage<string, ModalButtonData, ModalButtonData> { }
public class OpenNoticeModalMessage : AMessage<string, ModalButtonData> { }
public struct ModalButtonData
{
    public string Text;
    public Action PressCallback;
    public float DisableTime;
}
/// <summary>
/// Confirmation Modals are for showing choices
/// to the player, like yes/no
/// 
/// Examples: 
/// "Are you sure you want to delete save 4? -> Yes -> No"
/// "Do you want to dismantle this item? -> Yes -> No"
/// "Are you sure you want to exit to the desktop? -> Keep Playing -> Exit Game"
/// </summary>
public partial class ModalConfirmation : Node
{
    [Export] private ModalPanel modalPanel;
    [Export] private Label modalText;
    [Export] private Button cancelButton;
    [Export] private Button okButton;

    private OpenConfirmationModalMessage saveModalMessage;
    private ModalButtonData ok;
    private ModalButtonData cancel;

    private void OnEnable()
    {
        saveModalMessage = Messages.Get<OpenConfirmationModalMessage>();
        saveModalMessage.AddListener(OpenModal);
    }

    private void OnDisable()
    {
        saveModalMessage.RemoveListener(OpenModal);
        saveModalMessage = null;
        Messages.Return<OpenConfirmationModalMessage>();
    }

    public void OpenModal(string setModalText, ModalButtonData okModalData, ModalButtonData cancelModalData)
    {

        modalText.Text = setModalText;
        ok = okModalData;
        cancel = cancelModalData;


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

        if (cancel.DisableTime > 0)
        {
            cancelButton.Disabled = true;
            cancelButton.Text = $"{cancel.Text} ({cancel.DisableTime:F1})";
        }
        else
        {
            cancelButton.Disabled = false;
            cancelButton.Text = $"{cancel.Text}";
        }
        modalPanel.Visible = true;
        modalPanel.ProcessMode = ProcessModeEnum.Inherit;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void _Ready()
    {

        modalPanel.SetActive(false);
        cancelButton.Pressed += (CancelPressed);
        okButton.Pressed += (OkPressed);
    }
    private void CancelPressed()
    {
        cancel.PressCallback?.Invoke();
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

        if (cancel.DisableTime > 0)
        {
            cancel.DisableTime -= (float)delta;
            cancelButton.Disabled = true;
            cancelButton.Text = $"{cancel.Text} ({cancel.DisableTime:F1})";
            if (cancel.DisableTime <= 0)
            {
                cancelButton.Disabled = false;
                cancelButton.Text = $"{cancel.Text}";
            }
        }
    }
}
