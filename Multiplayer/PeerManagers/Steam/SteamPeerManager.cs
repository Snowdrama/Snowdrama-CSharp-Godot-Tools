using Godot;
using Steam;
using Steamworks;
using Steamworks.Data;
using System;

public partial class SteamPeerManager : IPeerManager
{
    string lobbyName;
    int maxPlayers;
    bool friendsOnly;

    bool isServer;
    uint lobbyId;

    public string GetIP()
    {
        throw new NotImplementedException();
    }

    public int GetPort()
    {
        throw new NotImplementedException();
    }

    public void InitializePeerManager()
    {
        if (isServer)
        {
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        }
        else
        {
            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        }
    }

    private void OnLobbyEntered(Lobby obj)
    {
        GD.Print($"Lobby Entered! {obj.Id}, Owner: {obj.Owner}, Data: {obj.Data}");
    }

    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        GD.Print($"Lobby Created!: [{result}, LobbyId: {lobby.Id}, Owner: {lobby.Owner}, Members: {lobby.MemberCount}]");
    }

    public bool IsServer()
    {
        return isServer;
    }

    public async void LaunchClient()
    {
        var lobby = await SteamMatchmaking.JoinLobbyAsync(lobbyId);
        if (lobby != null)
        {
            var steamPeer = new SteamMultiplayerPeer();
            steamPeer.CreateClient(SteamAPIGlobal.playerSteamId, lobby.Value.Owner.Id);
            MultiplayerLobby.SetPeer(steamPeer);
        }
        else
        {
            Debug.LogError($"We couldn't joing the lobby with id {lobbyId}");
        }

    }

    public async void LaunchServer()
    {
        Lobby? lobby = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
        if (lobby != null)
        {
            Debug.Log($"Lobby: {lobby.Value.Id}");
            lobby.Value.SetData("gm", MultiplayerGlobals.GameFilterKey);
            lobby.Value.SetData("name", lobbyName);
            if (friendsOnly)
            {
                lobby.Value.SetFriendsOnly();
            }
            else
            {
                lobby.Value.SetPublic();
            }
            lobby.Value.SetJoinable(true);


            var steamPeer = new SteamMultiplayerPeer();

            steamPeer.CreateHost(SteamAPIGlobal.playerSteamId);
            MultiplayerLobby.SetPeer(steamPeer);
        }
        else
        {
            Debug.Log($"Lobby is null!");
        }
    }

    public void SetIP(string ip)
    {
        throw new NotImplementedException();
    }

    public void SetPort(int port)
    {
        throw new NotImplementedException();
    }

    public void SetServer(bool isServer)
    {
        this.isServer = isServer;
    }

    public void ShutdownPeerManager()
    {
        throw new NotImplementedException();
    }

    public void SetJoinId(uint joinLobbyId)
    {
        this.lobbyId = joinLobbyId;
    }

    internal void SetMaxPlayers(int v)
    {
        this.maxPlayers = v;
    }

    internal void SetFriendsOnly(bool friendsOnly)
    {
        this.friendsOnly = friendsOnly;
    }

    internal void SetLobbyName(string lobbyName)
    {
        this.lobbyName = lobbyName;
    }
}
