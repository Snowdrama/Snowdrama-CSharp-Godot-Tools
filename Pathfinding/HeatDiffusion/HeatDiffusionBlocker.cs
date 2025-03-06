using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class HeatDiffusionBlocker : Node2D
{
    [Export] public Array<Vector2> blockerOffsets = new Array<Vector2>();
}
