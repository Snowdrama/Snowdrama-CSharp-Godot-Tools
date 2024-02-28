using Godot;
using System;


[GlobalClass]
public partial class TwitchLoginResource : Resource
{
    [Export] public string ChannelName { get; private set; }
    [Export] public string ChannelTwitchId { get; private set; }
    [Export(PropertyHint.Password)] public string APP_CLIENT_ID { get; private set; }
    [Export(PropertyHint.Password)] public string APP_CLIENT_SECRET { get; private set; }
    [Export(PropertyHint.Password)] public string OAUTH_CLIENT_ID { get; private set; }
    [Export(PropertyHint.Password)] public string OAUTH_ACCESS_TOKEN { get; private set; }
}
