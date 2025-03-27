using Godot;
using Godot.Collections;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;

public partial class SteamLobbySelect : ScrollContainer
{
    [Export] SteamMenu steamMenu;
    [Export] Button refreshButton;
    public override void _Ready()
    {
        base._Ready();

        this.VisibilityChanged += OnVisibilityChanged;
        refreshButton.Pressed += Refresh;
    }

    private void OnVisibilityChanged()
    {
        Refresh();
    }

    List<Button> buttons = new List<Button>();
    private async void Refresh()
    {
        var lobbies = await SteamMatchmaking.LobbyList.WithKeyValue("gm", MultiplayerGlobals.GameFilterKey).FilterDistanceWorldwide().RequestAsync();

        foreach (var button in buttons)
        {
            button.QueueFree();
        }
        buttons.Clear();
        foreach (var item in lobbies)
        {
            var name = item.GetData("name");
            var lobbyButton = new Button();
            lobbyButton.Text = $"{name} | Players[{item.MemberCount}/{item.MaxMembers}]";
            lobbyButton.AnchorLeft = 0;
            lobbyButton.AnchorRight = 1;
            lobbyButton.OffsetLeft = 0;
            lobbyButton.OffsetRight = 0;
            lobbyButton.Pressed += () => { steamMenu.JoinLobby((uint)item.Id.Value); };
            buttons.Add(lobbyButton);
            this.AddChild(lobbyButton);
        }
    }
}
