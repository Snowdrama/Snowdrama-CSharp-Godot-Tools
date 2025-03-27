using Godot;
using System;

public partial class EnetMenu : Node
{
    [Export] LineEdit playerNameLineEdit;

    [ExportCategory("Host")]
    [Export] Button hostButton;
    [Export] LineEdit hostPortLineEdit;

    [ExportCategory("Join")]
    [Export] Button joinButton;
    [Export] LineEdit joinIPLineEdit;
    [Export] LineEdit joinPortLineEdit;

    [ExportCategory("Start")]
    [Export] Button startButton;
    [Export] Button cancelButton;

    bool isServer;
    bool ready;

    public override void _Ready()
    {
        base._Ready();

        hostButton.Pressed += HostButton_Pressed;
        joinButton.Pressed += JoinButton_Pressed;
        startButton.Pressed += StartButton_Pressed;
        cancelButton.Pressed += CancelButton_Pressed;


        hostButton.Disabled = false;
        joinButton.Disabled = false;
        startButton.Disabled = true;
        cancelButton.Disabled = true;
    }


    private void JoinButton_Pressed()
    {
        isServer = false;
        ready = true;
        UpdateInputs();

    }

    private void HostButton_Pressed()
    {
        isServer = true;
        ready = true;
        UpdateInputs();
    }
    
    private void StartButton_Pressed()
    {
        var enetPeerManager = new ENetPeerManager();
        if (isServer)
        {
            enetPeerManager.SetPort(hostPortLineEdit.Text.ToInt());
            enetPeerManager.SetServer(true);
        }
        else
        {
            enetPeerManager.SetPort(joinPortLineEdit.Text.ToInt());
            enetPeerManager.SetIP(joinIPLineEdit.Text);
            enetPeerManager.SetServer(false);
        }
        MultiplayerLobby.SetPeerManager(enetPeerManager);

        //TODO: Make it so that I don't statically set this to game scene
        SceneManager.LoadScene("GameScene");

    }

    private void CancelButton_Pressed()
    {
        ready = false;
        UpdateInputs();
    }
    private void UpdateInputs()
    {
        //disables the buttons and line edits
        //once we host or join a game
        //if we cancel they become editable again
        hostButton.Disabled = ready;
        hostPortLineEdit.Editable = !ready;

        joinButton.Disabled = ready;
        joinIPLineEdit.Editable = !ready;
        joinPortLineEdit.Editable = !ready;


        startButton.Disabled = !ready;
        cancelButton.Disabled = !ready;
    }
}