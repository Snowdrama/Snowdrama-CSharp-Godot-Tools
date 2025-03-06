using Godot;
using Snowdrama.Core;
using System;

[GlobalClass]
public partial class HeatDiffusionObject : Node2D
{
    [Export] public float generatedHeat = 1.0f;
}
