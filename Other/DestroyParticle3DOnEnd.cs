using Godot;
using System;

public partial class DestroyParticle3DOnEnd : GpuParticles3D
{
	public override void _Process(double delta)
	{
		if (!Emitting)
		{
			this.QueueFree();
		}
	}
}
