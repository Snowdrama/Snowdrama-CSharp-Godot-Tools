using Godot;
using System;
using System.Drawing;
using System.Linq;

[Tool, GlobalClass, GodotClassName("RichTextEffectGhost")]
public partial class RichTextEffectGhost : RichTextEffect
{
    public string bbcode = "ghost";
    public override bool _ProcessCustomFX(CharFXTransform charFX)
    {
        var speed = charFX.Env["freq"].AsDouble();
        var span = charFX.Env["span"].AsDouble();

        var alpha = Mathf.Sin(charFX.ElapsedTime * speed + (charFX.Range.X / span)) * 0.5 + 0.5;

        var color = charFX.Color;
        color.A = (float)alpha;
        charFX.Color = color;
        return true;
    }
}
