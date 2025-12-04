using Godot;
using Godot.Collections;
using System;

public partial class HeatDiffusionBlocker : Node2D, IHeatDiffusionBlocker
{
    public virtual Array<Vector2> GetBlockerPoints()
    {
        throw new NotImplementedException();
    }
}
