using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TwitchLib.Api.Helix.Models.Bits;
using TwitchLib.PubSub.Events;

public enum AlertType
{
    Follow,
    Subscriber,
    Bits,
    Points,
}
public struct AlertData
{
    public int AlertId;
    public string Message;
    public AlertType AlertType;
    public string UserName;
    public string UserMessage;
}

public partial class AlertNode : Node
{
    Random rand = new Random();
    [ExportCategory("Global")]
    [Export] Gradient fadeGradient;
    [Export] AnimatedTextureRect animatedImage;
    [Export] RichTextLabel nameLabel;
    TwicthAlertMessage onAlert { get; set; }

    Queue<AlertData> alerts = new Queue<AlertData>();
    AlertData currentAlert;

    [ExportCategory("Timing")]
    float subAlpha;
    [Export] float subAlpha_Max = 1.0f;
    float subWaitTime = 0;
    [Export] float subWaitTime_Max = 1.0f;

    [ExportCategory("Alert Popup Data")]
    [Export] AlertPopupResourceDatabase alertPopupResourceDatabase;

    [ExportCategory("Audio")]
    [Export] AudioStreamPlayer2D audioPlayer;

    public override void _EnterTree()
    {
        onAlert = Messages.Get<TwicthAlertMessage>();
        onAlert.AddListener(OnAlert);
    }

    public override void _ExitTree()
    {
        onAlert.RemoveListener(OnAlert);
        Messages.Return<TwicthAlertMessage>();
    }
    public void OnAlert(AlertData data)
    {
        alerts.Enqueue(data);
    }

    public override void _Ready()
    {
        voices = DisplayServer.TtsGetVoicesForLanguage("en");
        GD.Print($"Voice Count: {voices.Length}");
        if (voices.Length > 0)
        {
            for (int i = 0; i < voices.Length; i++)
            {
                GD.Print($"{voices[i]}");

                //we always want to use catherine if we have it available
                if (voices[i].Contains("CATHERINE"))
                {
                    voiceId = voices[i];
                }
                
                //go with Zira if we haven't found something
                if (string.IsNullOrEmpty(voiceId) && voices[i].Contains("ZIRA"))
                {
                    voiceId = voices[i];
                }
            }
            if (string.IsNullOrEmpty(voiceId))
            {
                //if we didn't find Zira or Catherine... then use voice 0
                voiceId = voices[0];
            }
        }
    }

    string[] voices = DisplayServer.TtsGetVoicesForLanguage("en");
    string voiceId;
    public override void _Process(double delta)
    {
        switch (subAnimationState)
        {
            case FollowState.None:
                if (alerts.Count > 0)
                {
                    currentAlert = alerts.Dequeue();
                    nameLabel.Text = "[center]";
                    nameLabel.Text += $"[color={new Color(0.0f, 0.5f, 1.0f).ColorToHex()}]";
                    nameLabel.Text += currentAlert.Message;
                    nameLabel.Text += $"[/color]";
                    nameLabel.Text += "\n";
                    nameLabel.Text += currentAlert.UserMessage;
                    nameLabel.Text += "[/center]";
                    switch (currentAlert.AlertType)
                    {
                        case AlertType.Follow:
                            var followAlert = alertPopupResourceDatabase.followPopupResources.GetRandom();
                            animatedImage.AnimationName = followAlert.animationName;
                            audioPlayer.Stream = followAlert.audioStream;
                            audioPlayer.Play(0);
                            break;
                        case AlertType.Subscriber:
                            var subAlert = alertPopupResourceDatabase.subPopupResources.GetRandom();
                            animatedImage.AnimationName = subAlert.animationName;
                            audioPlayer.Stream = subAlert.audioStream;
                            audioPlayer.Play(0);
                            break;
                        case AlertType.Bits:
                            var bitsAlert = alertPopupResourceDatabase.bitsPopupResources.GetRandom();
                            animatedImage.AnimationName = bitsAlert.animationName;
                            audioPlayer.Stream = bitsAlert.audioStream;
                            audioPlayer.Play(0);
                            break;
                        case AlertType.Points:
                            var pointsAlert = alertPopupResourceDatabase.pointsPopupResources.GetRandom();
                            animatedImage.AnimationName = pointsAlert.animationName;
                            audioPlayer.Stream = pointsAlert.audioStream;
                            audioPlayer.Play(0);

                            break;
                    }
                    subAnimationState = FollowState.Showing;
                }
                break;
            case FollowState.Showing:
                subAlpha += (float)delta;
                if (subAlpha > subAlpha_Max)
                {
                    subAlpha = 1.0f;
                    subAnimationState = FollowState.PlayingSound;
                }
                break;
            case FollowState.PlayingSound:
                if (!audioPlayer.Playing)
                {
                    DisplayServer.TtsSpeak(currentAlert.UserMessage, voiceId);
                    subAnimationState = FollowState.PlayingTTS;
                }
                break;
            case FollowState.PlayingTTS:
                if (!DisplayServer.TtsIsSpeaking())
                {
                    subAnimationState = FollowState.Displayed;
                }
                break;
            case FollowState.Displayed:
                subWaitTime += (float)delta;
                if (subWaitTime > subWaitTime_Max)
                {
                    subWaitTime = 0;
                    subAnimationState = FollowState.Hiding;
                }
                break;
            case FollowState.Hiding:
                subAlpha -= (float)delta;
                if (subAlpha <= 0.0f)
                {
                    subAlpha = 0.0f;
                    subAnimationState = FollowState.None;
                }
                break;
        }
        animatedImage.Modulate = fadeGradient.Sample(subAlpha);
        nameLabel.Modulate = fadeGradient.Sample(subAlpha);
    }
    FollowState subAnimationState;
    enum FollowState
    {
        None,
        Showing,
        PlayingSound,
        PlayingTTS,
        Displayed,
        Hiding,
    }
}
