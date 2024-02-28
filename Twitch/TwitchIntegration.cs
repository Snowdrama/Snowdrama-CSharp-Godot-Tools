using Godot;
using System;

using System.Collections.Generic;
using System.Threading.Tasks;

//Chat is stinky
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;


//Follows/Subs/Bits/Points/Raids
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Models;
using TwitchLib.PubSub.Models.Responses.Messages.AutomodCaughtMessage;

using PredictionStatus = TwitchLib.PubSub.Enums.PredictionStatus;

using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomRewardRedemptionStatus;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Core;

public class OnBitsReceivedMessage : AMessage<string, OnBitsReceivedV2Args> { }
public class OnChannelPointsMessage : AMessage<string, OnChannelPointsRewardRedeemedArgs> { }
public class OnFollowMessage : AMessage<string, OnFollowArgs> { }
public class OnChannelSubscriptionMessage : AMessage<string, OnChannelSubscriptionArgs> { }
public class OnMessageReceivedMessage : AMessage<string, OnMessageReceivedArgs> { }


[GlobalClass]
public partial class TwitchIntegration : Node
{
    [Export]
    private TwitchLoginResource TwitchLoginResource { get; set; }
    private TwitchPubSub pubSubClient;
    private TwitchClient chatClient;
    public static ITwitchAPI API;

    
    OnChannelPointsMessage onChannelPointsMessage;
    OnBitsReceivedMessage onBitsReceivedMessage;
    OnChannelSubscriptionMessage onChannelSubscriptionMessage;
    OnFollowMessage onFollowMessage;

    public override void _EnterTree()
    {
        API = new TwitchAPI(settings: new ApiSettings()
        {
            ClientId = TwitchLoginResource.APP_CLIENT_ID,
            Secret = TwitchLoginResource.APP_CLIENT_SECRET,
            Scopes = new List<AuthScopes>()
                {
                    AuthScopes.Chat_Read,
                    AuthScopes.User_Read,
                    //Follows
                    AuthScopes.Helix_User_Read_Follows,
                    //Bits
                    AuthScopes.Helix_Bits_Read,
                    //Subs
                    AuthScopes.Helix_Channel_Read_Subscriptions,
                    //points
                    AuthScopes.Helix_Channel_Read_Redemptions,
                    AuthScopes.Helix_Channel_Manage_Redemptions,

                    AuthScopes.Helix_Channel_Read_Goals,
                    AuthScopes.Helix_Channel_Read_Hype_Train,
                    AuthScopes.Helix_Channel_Read_Polls,
                    AuthScopes.Helix_Channel_Read_Predictions,
                }
        });

        GD.Print($"ClientID: {TwitchLoginResource.APP_CLIENT_ID}");
        GD.Print($"APP_CLIENT_SECRET: {TwitchLoginResource.APP_CLIENT_SECRET}");

        GD.Print("Starting Chat Client");
        ConnectionCredentials credentials = new ConnectionCredentials(TwitchLoginResource.ChannelName, TwitchLoginResource.APP_CLIENT_SECRET);
        var clientOptions = new ClientOptions
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };

        WebSocketClient customClient = new WebSocketClient(clientOptions);

        chatClient = new TwitchClient(customClient);

        chatClient.Initialize(credentials, TwitchLoginResource.ChannelName);

        //chatClient.OnLog += ChatClient_OnLog;
        chatClient.OnJoinedChannel += ChatClient_OnJoinedChannel;
        chatClient.OnMessageReceived += ChatClient_OnMessageReceived;
        chatClient.OnNewSubscriber += ChatClient_OnNewSubscriber;
        chatClient.OnConnected += ChatClient_OnConnected;


        GD.Print("Starting PubSub Client");
        //Follows/Subs/Bits/Points/Raids
        pubSubClient = new TwitchPubSub();
        pubSubClient.OnLog += PubSubClient_OnLog;
        pubSubClient.OnPubSubServiceError += PubSubClient_OnPubSubServiceError;
        pubSubClient.OnPubSubServiceClosed += PubSubClient_OnPubSubServiceClosed;

        pubSubClient.ListenToFollows(TwitchLoginResource.ChannelTwitchId);
        pubSubClient.ListenToRaid(TwitchLoginResource.ChannelTwitchId);
        pubSubClient.ListenToSubscriptions(TwitchLoginResource.ChannelTwitchId);
        pubSubClient.ListenToBitsEventsV2(TwitchLoginResource.ChannelTwitchId);

        pubSubClient.ListenToChannelPoints(TwitchLoginResource.ChannelTwitchId);
        //pubSubClient.ListenToRewards(TwitchLoginResource.ChannelTwitchId);


        pubSubClient.OnPubSubServiceConnected += OnPubSubServiceConnected;
        pubSubClient.OnListenResponse += OnListenResponse;
        pubSubClient.OnStreamUp += OnStreamUp;
        pubSubClient.OnStreamDown += OnStreamDown;
        pubSubClient.OnChannelPointsRewardRedeemed += OnChannelPointsRewardRedeemed;
        
        pubSubClient.OnBitsReceivedV2 += OnBitsReceivedV2;

        pubSubClient.OnFollow += PubSubClient_OnFollow;
        pubSubClient.OnChannelSubscription += PubSubClient_OnChannelSubscription;
        pubSubClient.OnRaidUpdateV2 += PubSubClient_OnRaidUpdateV2;




        onChannelPointsMessage = Messages.Get<OnChannelPointsMessage>();
        onBitsReceivedMessage = Messages.Get<OnBitsReceivedMessage>();
        onChannelSubscriptionMessage = Messages.Get<OnChannelSubscriptionMessage>();
        onFollowMessage = Messages.Get<OnFollowMessage>();
    }

    private void PubSubClient_OnPubSubServiceClosed(object sender, EventArgs e)
    {
        GD.PrintErr($"PubSub CLOSED {sender}: {e}");
    }

    public override void _ExitTree()
    {
        pubSubClient.Disconnect();
        pubSubClient = null;
        Messages.Return<OnChannelPointsMessage>();
        Messages.Return<OnBitsReceivedMessage>();
        Messages.Return<OnChannelSubscriptionMessage>();
        Messages.Return<OnFollowMessage>();
    }

    public override void _Ready()
    {
        chatClient.Connect();
        GD.Print("Connecting Chat Client");

        pubSubClient.Connect();
        GD.Print("Connecting PubSub Client");
    }

    private void OnPubSubServiceConnected(object sender, EventArgs e)
    {
        GD.Print("Sending Topics");
        pubSubClient.SendTopics(TwitchLoginResource.OAUTH_ACCESS_TOKEN);
    }

    private void OnListenResponse(object sender, OnListenResponseArgs e)
    {
        if (!e.Successful)
            throw new Exception($"Failed to listen! Response: {e.Response}");
    }

    private void OnStreamUp(object sender, OnStreamUpArgs e)
    {
        GD.Print($"Stream just went up! Play delay: {e.PlayDelay}, server time: {e.ServerTime}");
    }

    private void OnStreamDown(object sender, OnStreamDownArgs e)
    {
        GD.Print($"Stream just went down! Server time: {e.ServerTime}");
    }

    private void OnChannelPointsRewardRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
    {
        GD.Print($"Channel Points Redeemed!");
        GD.Print($"Dispatching Redeemed Args!");

        var redemption = e.RewardRedeemed.Redemption;
        var reward = e.RewardRedeemed.Redemption.Reward;
        var redeemedUser = e.RewardRedeemed.Redemption.User;
        onChannelPointsMessage.Dispatch(redeemedUser.DisplayName, e);
    }

    private void OnBitsReceivedV2(object sender, OnBitsReceivedV2Args e)
    {
        GD.Print($"Bits Received!!");
        onBitsReceivedMessage.Dispatch(e.UserName, e);
    }

    private void PubSubClient_OnRaidUpdateV2(object sender, OnRaidUpdateV2Args e)
    {
        GD.Print($"Raid Received?");
        GD.Print($"Sender: {(string)sender}");
        GD.Print($"Viewer Count: {e.ViewerCount}");
        GD.Print($"Target Display Name: {e.TargetDisplayName}");
        GD.Print($"Sender? Channel Id: {e.ChannelId}");
        GD.Print($"Target Channel Id: {e.TargetChannelId}");
    }

    private void PubSubClient_OnChannelSubscription(object sender, OnChannelSubscriptionArgs e)
    {
        onChannelSubscriptionMessage.Dispatch(e.Subscription.DisplayName, e);
    }

    private void PubSubClient_OnFollow(object sender, OnFollowArgs e)
    {
        GD.Print($"Follow Received!!");
        onFollowMessage.Dispatch(e.DisplayName, e);
    }

    private void ChatClient_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
    {
        GD.Print($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
    }

    private void PubSubClient_OnLog(object sender, TwitchLib.PubSub.Events.OnLogArgs e)
    {
        GD.Print($"PubSub {sender}: {e.Data}");
    }
    private void PubSubClient_OnPubSubServiceError(object sender, OnPubSubServiceErrorArgs e)
    {
        GD.PrintErr($"PubSubError {sender}: {e.Exception}");
    }

    private void ChatClient_OnConnected(object sender, OnConnectedArgs e)
    {
    }

    private void ChatClient_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        //GD.Print("Hey guys! I am a bot connected via TwitchLib!");
        //chatClient.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
    }

    OnMessageReceivedMessage onMessageReceivedMessage;
    private void ChatClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        if(onMessageReceivedMessage == null)
        {
            onMessageReceivedMessage = Messages.Get<OnMessageReceivedMessage>();
        }
        onMessageReceivedMessage.Dispatch((string)sender, e);
    }

    private void ChatClient_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
    {
    }
}
