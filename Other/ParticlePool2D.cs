using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;


public class PlayParticle2DMessage : AMessage<string, Vector2> { }

public partial class ParticlePool2D : Node
{
    [Export] string particleName = "Explosion1";
    Array<GpuParticles2D> particles = new Array<GpuParticles2D>();
    Stack<GpuParticles2D> particlesStack = new Stack<GpuParticles2D>();
    public override void _Ready()
    {
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            var child = this.GetChild(i);
            if (child is GpuParticles2D p)
            {
                particles.Add(p);

                particlesStack.Push(p);

                p.Finished += () =>
                {
                    particlesStack.Push(p);
                };
            }
        }
    }

    public virtual void PlayParticle(string targetParticleName, Vector2 position)
    {
        if (particleName != targetParticleName) { return; }
        Debug.Log($"Placing Piece At {position}");
        var particle = particlesStack.Pop();
        particle.GlobalPosition = position;
        particle.Restart();
    }

    PlayParticle2DMessage playParticle2DMessage;
    public override void _EnterTree()
    {
        base._EnterTree();

        playParticle2DMessage = Messages.Get<PlayParticle2DMessage>();
        playParticle2DMessage.AddListener(PlayParticle);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        playParticle2DMessage.RemoveListener(PlayParticle);
        Messages.Return<PlayParticle2DMessage>();
    }
}
