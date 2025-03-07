using Godot;
using Godot.Collections;

interface IHeatDiffusionBlocker
{
    public Array<Vector2> GetBlockerPoints();
}
