﻿using Godot;

/// <summary>
/// Plays a message from the sound pool with a certain clip, at a certain point and with 
/// a specific bus for volume settings like "Footsteps" or "Voice"
/// </summary>
public class PlayPosiotionedSoundMessage : AMessage<AudioStream, Vector2, string> { }


/// <summary>
/// Plays a message from the sound pool with a certain clip and with 
/// a specific bus for volume settings like "Footsteps" or "Voice"
/// </summary>
public class PlaySoundMessage : AMessage<AudioStream, string> { }
