using Godot;
using System;

/// <summary>
/// An example of an actor for a game, like a 2D character on the world 
/// 
/// This example is intended to be generic but is roughly based on 2D games like
/// 
/// 
/// Golden Sun
/// YS 1 & 2
/// 
/// Could also be used by 2D sidescrollers like:
/// 
/// Shantae
/// 
/// This covers some basic things like:
/// 
/// MoveToLocation
/// FaceDirection
/// Emote
/// 
/// </summary>

public enum ActorGame2DEmoteType
{
    None,
    Happy,
    Sad,
    Angry,
}

public partial class ActorGame2D : Node2D
{
    public void MoveToLocation()
    {

    }

    public void FaceDirection()
    {

    }

    public void Emote(ActorGame2DEmoteType emoteType)
    {
        
    }
}
