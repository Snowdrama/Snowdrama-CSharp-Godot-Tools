﻿using Godot;
/// <summary>
/// Plays a message from the sound pool with a certain clip, at a certain point and with 
/// a specific bus for volume settings like "Sounds" or "Voice"
/// 
/// AudioStream - The stream to play
/// Vector3 - the position in 3D space to play the sound
/// string - The Bus name to use, like "Sounds" or "Voice"
/// float -  The volume to play the sound at, this does not effect the bus volume
/// </summary>
public class PlayPosiotionedSoundMessage3D : AMessage<AudioStream, Vector3, string, float, Vector2> { }
