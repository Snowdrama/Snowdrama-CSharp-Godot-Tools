using Godot;
using Godot.Collections;
using System.Collections.Generic;


public class PlayParticle2DMessage : AMessage<string, Vector2, Vector2> { }

public partial class ParticlePool2D : Node
{
    PlayParticle2DMessage? playParticle2DMessage;

    [Export] string particleName = "Explosion1";
    Array<GpuParticles2D> particles = new Array<GpuParticles2D>();
    Stack<GpuParticles2D> particlesStack = new Stack<GpuParticles2D>();

    [Export] int particleCount = 16;
    [Export] PackedScene particlePrefab = null!;

    public override void _Ready()
    {
        for (int i = 0; i < particleCount; i++)
        {
            if (particlePrefab != null)
            {
                var newParticle = particlePrefab.Instantiate() as GpuParticles2D;
                if (newParticle != null && newParticle is GpuParticles2D)
                {
                    newParticle.OneShot = true;
                    newParticle.Emitting = false;
                    this.AddChild(newParticle);
                    particlesStack.Push(newParticle);
                    newParticle.Finished += () =>
                    {
                        particlesStack.Push(newParticle);
                    };
                }
                else
                {
                    Debug.LogError($"ParticlePrefab on {this.Name} is null or not a GpuParticles2D!");
                }
            }
            else
            {
                Debug.LogError($"ParticlePrefab on {this.Name} is null!");
            }
        }
    }

    public virtual void PlayParticle(string targetParticleName, Vector2 position, Vector2 direction)
    {
        if (particleName != targetParticleName) { return; }
        //Debug.Log($"{this.Name} is Playing Particle: {targetParticleName} at {position}!");
        if (particlesStack.Count > 0)
        {
            var particle = particlesStack.Pop();
            particle.GlobalPosition = position;
            particle.GlobalRotation = direction.AngleFromVectorRads();
            particle.Restart();
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        playParticle2DMessage = Messages.Get<PlayParticle2DMessage>();
        playParticle2DMessage.AddListener(PlayParticle);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        playParticle2DMessage?.RemoveListener(PlayParticle);
        Messages.Return<PlayParticle2DMessage>();
    }
}
