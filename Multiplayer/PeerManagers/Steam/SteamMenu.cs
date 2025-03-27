using Godot;
using System;

public partial class SteamMenu : Node
{

    [ExportCategory("Create Lobby")]
    [Export] LineEdit lobbyNameLineEdit;
    [Export] LineEdit maxPlayersLineEdit;
    [Export] CheckButton friendsOnlyCheckButton;
    [Export] Button createGameButton;

    [ExportCategory("Join Lobby")]
    [Export] LineEdit joinLobbyIdLineEdit;
    [Export] Button joinLobbyWithIdButton;
    public override void _Ready()
    {
        base._Ready();
        createGameButton.Pressed += OnCreateGame;
        joinLobbyWithIdButton.Pressed += JoinLobbyWithId;
    }
    private void OnCreateGame()
    {
        var steamPeerManager = new SteamPeerManager();
        steamPeerManager.SetLobbyName(lobbyNameLineEdit.Text);
        steamPeerManager.SetMaxPlayers(maxPlayersLineEdit.Text.ToInt());
        steamPeerManager.SetFriendsOnly(friendsOnlyCheckButton.ButtonPressed);
        steamPeerManager.SetServer(true);
        MultiplayerLobby.SetPeerManager(steamPeerManager);

        //TODO: Make it so that I don't statically set this to game scene
        SceneManager.LoadScene("GameScene");
    }
    private void JoinLobbyWithId()
    {
        var lobbyId = (uint)joinLobbyIdLineEdit.Text.ToInt();
        JoinLobby(lobbyId);
    }
    public void JoinLobby(uint lobbyId)
    {
        var steamPeerManager = new SteamPeerManager();
        steamPeerManager.SetJoinId(lobbyId);
        steamPeerManager.SetServer(false);
        MultiplayerLobby.SetPeerManager(steamPeerManager);
        //TODO: Make it so that I don't statically set this to game scene
        SceneManager.LoadScene("GameScene");
    }
}
