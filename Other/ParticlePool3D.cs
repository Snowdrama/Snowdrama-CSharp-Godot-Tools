using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class PlayParticle3DMessage : AMessage<Vector3> { }
public partial class ParticlePool3D : Node
{
    Array<GpuParticles3D> particles = new Array<GpuParticles3D>();
    Stack<GpuParticles3D> particlesStack = new Stack<GpuParticles3D>();
    public override void _Ready()
    {
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            var child = this.GetChild(i);
            if (child is GpuParticles3D p)
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

    public virtual void PlayParticle(Vector3 position)
    {
        var particle = particlesStack.Pop();
        particle.GlobalPosition = position;
        particle.OneShot = true;
        particle.Restart();
        //Debug.Log($"Playing Particle {particle.Name}");
    }

    PlayParticle3DMessage playParticle3DMessage;
    public override void _EnterTree()
    {
        base._EnterTree();
        playParticle3DMessage = Messages.Get<PlayParticle3DMessage>();
        playParticle3DMessage.AddListener(PlayParticle);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        playParticle3DMessage.RemoveListener(PlayParticle);
        Messages.Return<PlayParticle3DMessage>();
    }
}
