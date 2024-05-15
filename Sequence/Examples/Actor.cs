using Godot;
using Godot.Collections;
using Snowdrama.Spring;
using System;
using System.Security;
public partial class Actor : Control
{
	[Export] Array<ActorEmote> emotes = new Array<ActorEmote>();
	Dictionary<string, Texture2D> namedEmotes = new Dictionary<string, Texture2D>();
	[Export] TextureRect characterPortrait;


    [Export] SpringConfigurationResource positionSpringConfig;
    Spring2D positionSpring;


    bool visible = false;
    [Export] SpringConfigurationResource imageAlphaSpringConfig;
    Spring imageAlphaSpring;
    public override void _Ready()
    {
        positionSpring = new Spring2D(positionSpringConfig.Config);
        imageAlphaSpring = new Spring(imageAlphaSpringConfig.Config);
        for (int i = 0; i < emotes.Count; i++)
		{
			namedEmotes.Add(emotes[i].emoteName, emotes[i].emoteTexture);
		}
		characterPortrait.Texture = emotes[0].emoteTexture;
	}

	public override void _Process(double delta)
    {
		positionSpring.Update(delta);
        this.GlobalPosition = positionSpring.Value;
    }


	public void ShowEmote(string emoteName)
	{
		if (namedEmotes.ContainsKey(emoteName))
		{
            characterPortrait.Texture = namedEmotes[emoteName];
		}
	}


	public void GoToPosition(Vector2 targetPosition)
	{
		positionSpring.Target = targetPosition;
    }



}
