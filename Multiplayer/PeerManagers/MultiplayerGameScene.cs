using Godot;
using System;

public partial class MultiplayerGameScene : Node
{

    public override void _EnterTree()
    {
        base._EnterTree();
        this.Multiplayer.ServerDisconnected += Multiplayer_ServerDisconnected;
        MultiplayerLobby.ConnectMultiplayer();
    }

    private void Multiplayer_ServerDisconnected()
    {
        Debug.Log("Server Disconnected! Returning to Main Menu!");
        SceneManager.LoadScene("MainMenu");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        this.Multiplayer.ServerDisconnected -= Multiplayer_ServerDisconnected;

        //if we're shutting down and we're the server, just send the close status
        if(this.Multiplayer.MultiplayerPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connected)
        {
            if (this.Multiplayer.IsServer())
            {
                this.Multiplayer.MultiplayerPeer.Close();
            }
        }
    }
}
