using Godot;
using Steamworks;
using System;

public partial class SteamAPIGlobal : Node
{
    public static bool IsOnSteamDeck;
    public static bool IsLoggedOn;
    public static SteamId playerSteamId;
    public static string steamName;
    public static FriendState currentState;
    public static bool isSubscribed;
    public override void _Ready()
    {
        base._Ready();
		try
		{
			SteamClient.Init(MultiplayerGlobals.SteamAppId, true);

            IsLoggedOn = SteamClient.IsLoggedOn;
            playerSteamId = SteamClient.SteamId;
            steamName = SteamClient.Name;
            currentState = SteamClient.State;
            isSubscribed = SteamApps.IsSubscribed;

        }
		catch (Exception e)
		{
			GD.PrintErr(e);
			throw;
		}
    }
}
