using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ParticlePool3D : Node
{
	[Export] Array<GpuParticles3D> particles = new Array<GpuParticles3D>();

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
        //GD.Print($"Playing Particle {particle.Name}");
	}
}
