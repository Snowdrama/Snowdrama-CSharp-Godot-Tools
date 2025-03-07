using Godot;
using System;

[Tool, GlobalClass]
public partial class AnimatedTextureRect : TextureRect
{
    SpriteFrames _spriteFrames;
    string _animationName = "default";
    [Export]
    public SpriteFrames SpriteFrames
    {
        get { return _spriteFrames; }
        set
        {
            _spriteFrames = value;
            UpdateTexture();
        }
    }
    [Export]
    public string AnimationName
    {
        get { return _animationName; }
        set
        {
            _animationName = value;
            UpdateTexture();
        }
    }

    int _frame = 0;
    [Export]
    int Frame
    {
        get { return _frame; }
        set { _frame = value; }
    }

    int CurrentSpriteFrameCount = 0;
    double CurrentAnimationSpeed = 0;
    double frameTime = 1 / 12;
    [Export]
    bool playing = true;
    [Export]
    bool playingEditor = false;
    public override void _Process(double delta)
    {
        if ((Engine.IsEditorHint() && playingEditor) || (!Engine.IsEditorHint() && playing))
        {
            if (SpriteFrames != null && SpriteFrames.HasAnimation(AnimationName))
            {
                CurrentAnimationSpeed = 1.0f / SpriteFrames.GetAnimationSpeed(AnimationName);
                frameTime += delta;
                if (frameTime >= CurrentAnimationSpeed)
                {
                    frameTime = 0;
                    Frame++;
                    UpdateTexture();
                }
            }
        }
        else if(Engine.IsEditorHint())
        {
            UpdateTexture();
        }
    }

    public void UpdateTexture()
    {
        if (SpriteFrames != null && SpriteFrames.HasAnimation(AnimationName))
        {
            CurrentSpriteFrameCount = SpriteFrames.GetFrameCount(AnimationName);
            if(CurrentSpriteFrameCount > 0)
            {
                Frame = Frame.WrapClamp(0, CurrentSpriteFrameCount);
                this.Texture = SpriteFrames.GetFrameTexture(AnimationName, Frame);
            }
        }
    }
}
