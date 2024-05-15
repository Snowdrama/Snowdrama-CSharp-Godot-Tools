using Godot;
/// <summary>
/// The idea for this is that the Actor accepts messages from sequence steps
/// to change an element in a particular way
/// so rather than the step manipulating the sprite directly it might say... change the "emote"
/// or ask to animate to a spot.
/// 
/// Specifically this is for 2D VN style characters
/// where you would swap out the emote entirely.
/// 
/// </summary>


[GlobalClass]
public partial class ActorEmote : Resource
{
	[Export] public string emoteName;
    [Export] public Texture2D emoteTexture;
}
