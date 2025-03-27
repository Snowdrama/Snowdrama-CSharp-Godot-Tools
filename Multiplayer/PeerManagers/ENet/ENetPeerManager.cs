using Godot;
using System;
using System.Runtime.CompilerServices;

public interface IPeerManager
{
    public void SetPort(int port);
    public int GetPort();
    public void SetIP(string ip);
    public string GetIP();

    public void SetServer(bool isServer);
    public bool IsServer();

    public void LaunchServer();
    public void LaunchClient();

    public void InitializePeerManager();
    public void ShutdownPeerManager();
}

public partial class ENetPeerManager : Node, IPeerManager
{
    ENetMultiplayerPeer eNetPeer;
    bool isServer;
    string ip;
    int port;

    public string GetIP()
    {
        return ip;
    }

    public int GetPort()
    {
        return port;
    }

    public void SetIP(string ip)
    {
        this.ip = ip;
    }

    public void SetPort(int port)
    {
        this.port = port;
    }

    public void SetServer(bool isServer)
    {
        this.isServer = isServer;
    }

    public bool IsServer()
    {
        return isServer;
    }

    public void LaunchServer()
    {
        Debug.Log($"Launching Server on port: {port}");
        eNetPeer = new ENetMultiplayerPeer();
        eNetPeer.CreateServer(port);
        MultiplayerLobby.SetPeer(eNetPeer);
    }

    public void LaunchClient()
    {
        Debug.Log($"Launching Host on address: {ip}:{port}");
        eNetPeer = new ENetMultiplayerPeer();
        eNetPeer.CreateClient(ip, port);
        MultiplayerLobby.SetPeer(eNetPeer);
    }

    public void InitializePeerManager()
    {
    }

    public void ShutdownPeerManager()
    {
    }
}
