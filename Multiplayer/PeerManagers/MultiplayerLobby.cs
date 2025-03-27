using Godot;
using ImGuiNET;
using System;
using System.ComponentModel;

[GlobalClass]   
public partial class MultiplayerLobby : Node
{
    private static MultiplayerLobby instance;
    public static MultiplayerPeer multiplayerPeer;

    public static int peer_id = -1;
    public static int host_id = -1;

    public static IPeerManager peerManager;

    public override void _Ready()
    {
        base._Ready();
        if (instance != null)
        {
            this.QueueFree();
            return;
        }
        instance = this;

        this.Multiplayer.ConnectionFailed += Multiplayer_ConnectionFailed;
        this.Multiplayer.ServerDisconnected += Multiplayer_ServerDisconnected;
    }


    public static void ConnectMultiplayer()
    {
        if (peerManager.IsServer())
        {
            peerManager.LaunchServer();
        }
        else
        {
            peerManager.LaunchClient();
        }
    }

    public static void SetPeerManager(IPeerManager setPeerManager)
    {
        peerManager = setPeerManager;
        peerManager.InitializePeerManager();
    }

    public static void SetPeer(MultiplayerPeer setMultiplayerPeer)
    {
        multiplayerPeer = setMultiplayerPeer;
        instance.Multiplayer.MultiplayerPeer = multiplayerPeer;
    }

    private void Multiplayer_ConnectionFailed()
    {
        peerManager.ShutdownPeerManager();
    }
    private void Multiplayer_ServerDisconnected()
    {
        peerManager.ShutdownPeerManager();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(this.Multiplayer.MultiplayerPeer != null)
        {
            ImGui.Begin("Multiplayer");
            ImGui.Text($"Peer Status {this.Multiplayer.MultiplayerPeer.GetConnectionStatus()}");
            if(this.Multiplayer.MultiplayerPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connected)
            {
                ImGui.Text($"IsServer {this.Multiplayer.IsServer}");
            }
            ImGui.End();
        }
    }
}