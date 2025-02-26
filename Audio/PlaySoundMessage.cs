using Godot;


/// <summary>
/// Plays a message from the sound pool with a certain clip and with 
/// a specific bus for volume settings like "Sounds" or "Voice"
/// 
/// AudioStream - The stream to play
/// string - The Bus name to use, like "Sounds" or "Voice"
/// float -  The volume to play the sound at, this does not effect the bus volume
/// </summary>
public class PlaySoundMessage : AMessage<AudioStream, string, float, Vector2> { }
