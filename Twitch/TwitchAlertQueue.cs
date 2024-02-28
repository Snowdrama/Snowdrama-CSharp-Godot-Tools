using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.PubSub.Events;


public class TwicthAlertMessage : AMessage<AlertData> { }
public class CompletedAlertsChangedMessage : AMessage { }

[GlobalClass]
public partial class TwitchAlertQueue : Node
{
    Random rand = new Random();
    [ExportCategory("Follow")]
    [Export] string followMessage = "Thanks for the follow [NAME]";

    [ExportCategory("Subscription")]
    [Export] string subMessage = "Thanks for the sub [NAME]";

    [ExportCategory("Bits")]
    [Export] string bitsMessage = "Thanks [NAME] for the [BITS] bits!";

    [ExportCategory("Points")]
    [Export] string pointsMessage = "[NAME] Redeemed: \n [REWARD]";
    [Export] string pointsDetailsMessage = "Redeemed: \n  [REWARD]!";


    OnChannelSubscriptionMessage onSubscription { get; set; }
    OnFollowMessage onFollow { get; set; }
    OnBitsReceivedMessage onBits { get; set; }
    OnChannelPointsMessage onChannelPoints { get; set; }


    Queue<AlertData> completedQueue = new Queue<AlertData>();

    Queue<AlertData> waitingQueue = new Queue<AlertData>();

    int _lastAlertId = 0;
    [Export] bool demo = false;
    public override void _EnterTree()
    {
        onSubscription = Messages.Get<OnChannelSubscriptionMessage>();
        onSubscription.AddListener(NewSubscriber);
        onFollow = Messages.Get<OnFollowMessage>();
        onFollow.AddListener(NewFollow);
        onBits = Messages.Get<OnBitsReceivedMessage>();
        onBits.AddListener(BitsReceived);
        GD.Print("Registering Points Being Used");
        onChannelPoints = Messages.Get<OnChannelPointsMessage>();
        onChannelPoints.AddListener(PointsUsed);
    }

    public override void _ExitTree()
    {
        onSubscription.RemoveListener(NewSubscriber);
        Messages.Return<OnChannelSubscriptionMessage>();
        onFollow.RemoveListener(NewFollow);
        Messages.Return<OnFollowMessage>();
        onBits.RemoveListener(BitsReceived);
        Messages.Return<OnBitsReceivedMessage>();
        onChannelPoints.RemoveListener(PointsUsed);
        Messages.Return<OnChannelPointsMessage>();
    }

    public override void _Process(double delta)
    {
        if(waitingQueue.Count > 0)
        {
            var nextAlert = waitingQueue.Dequeue();

            Messages.Get<TwicthAlertMessage>().Dispatch(nextAlert);
            completedQueue.Enqueue(nextAlert);
            if(completedQueue.Count > 50)
            {
                completedQueue.Dequeue();
            }
            Messages.Get<CompletedAlertsChangedMessage>().Dispatch();
        }

        if (demo)
        {
            demo = false;
            DemoAlert();
        }
    }

    public int GetLastAlertId()
    {
        return _lastAlertId;
    }
    public Queue<AlertData> GetCompletedAlerts()
    {
        return completedQueue;
    }

    public void ReplayAlert(AlertData pastAlert)
    {
        Messages.Get<TwicthAlertMessage>().Dispatch(pastAlert);
    }

    public void BitsReceived(string sender, OnBitsReceivedV2Args e)
    {
        if (!e.IsAnonymous)
        {
            waitingQueue.Enqueue(new AlertData()
            {
                AlertId = _lastAlertId,
                Message = bitsMessage.Replace("[NAME]", e.UserName).Replace("[BITS]", $"{e.BitsUsed}"),
                AlertType = AlertType.Bits,
                UserName = e.UserName,
                UserMessage = e.ChatMessage,
            });
            _lastAlertId++;
        }
    }
    public void PointsUsed(string sender, OnChannelPointsRewardRedeemedArgs e)
    {
        GD.Print("Using Points!");
        waitingQueue.Enqueue(new AlertData()
        {
            AlertId = _lastAlertId,
            Message = pointsMessage.Replace("[NAME]", e.RewardRedeemed.Redemption.User.DisplayName).Replace("[REWARD]", $"{e.RewardRedeemed.Redemption.Reward.Title}"),
            AlertType = AlertType.Points,
            UserName = e.RewardRedeemed.Redemption.User.DisplayName,
            UserMessage = e.RewardRedeemed.Redemption.UserInput,
        });
        _lastAlertId++;
    }
    public void NewSubscriber(string sender, OnChannelSubscriptionArgs e)
    {
        waitingQueue.Enqueue(new AlertData()
        {
            AlertId = _lastAlertId,
            Message = subMessage.Replace("[NAME]", e.Subscription.DisplayName),
            AlertType = AlertType.Subscriber,
            UserName = e.Subscription.DisplayName,
            UserMessage = e.Subscription.SubMessage.Message
        });
        _lastAlertId++;
    }
    public void NewFollow(string sender, OnFollowArgs e)
    {
        waitingQueue.Enqueue(new AlertData()
        {
            AlertId = _lastAlertId,
            Message = followMessage.Replace("[NAME]", e.DisplayName),
            AlertType = AlertType.Follow,
            UserName = e.DisplayName,
            UserMessage = "",
        });
        _lastAlertId++;
    }

    public void DemoAlert()
    {
        var randomNumber = rand.Next(0, 4);
        var userNameTest = $"UserName_{rand.Next(0, 1_000_000_000)}";
        switch (randomNumber)
        {
            case 0:
                waitingQueue.Enqueue(new AlertData()
                {
                    AlertId = _lastAlertId,
                    Message = $"Thanks for the sub {userNameTest}",
                    AlertType = AlertType.Subscriber,
                    UserName = userNameTest,
                    UserMessage = $"This is a test message! A very long test message! This should play to the end and then wait 1 second and fade out!"
                });
                _lastAlertId++;
                break;
            case 1:

                waitingQueue.Enqueue(new AlertData()
                {
                    AlertId = _lastAlertId,
                    Message = $"Thanks for following {userNameTest}",
                    AlertType = AlertType.Follow,
                    UserName = userNameTest,
                    UserMessage = ""
                });
                _lastAlertId++;
                break;
            case 2:
                waitingQueue.Enqueue(new AlertData()
                {
                    AlertId = _lastAlertId,
                    Message = $"Thanks for the 400 bits {userNameTest}!",
                    AlertType = AlertType.Bits,
                    UserName = userNameTest,
                    UserMessage = "I love your streams!"
                });
                _lastAlertId++;
                break;
            case 3:
                waitingQueue.Enqueue(new AlertData()
                {
                    AlertId = _lastAlertId,
                    Message = $"{userNameTest} redeemed HilightMessage",
                    AlertType = AlertType.Points,
                    UserName = userNameTest,
                    UserMessage = "This is my message HILIGHT IT!"
                });
                _lastAlertId++;
                break;
        }
    }
}
