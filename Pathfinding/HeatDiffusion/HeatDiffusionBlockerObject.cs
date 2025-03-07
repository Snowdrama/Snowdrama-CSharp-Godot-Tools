using Godot;
using Godot.Collections;

[GlobalClass]
public partial class HeatDiffusionBlockerObject : HeatDiffusionBlocker
{
    [Export] HeatDiffusionMap heatMap;
    [Export] public Array<Vector2> blockerOffsets = new Array<Vector2>();
    [Export] public Array<Marker2D> blockerPoints = new Array<Marker2D>();

    [Export] float cellSize = 50;
    public override void _Ready()
    {
        base._Ready();

        for (int i = 0; i < blockerPoints.Count; i++)
        {
            blockerOffsets.Add(blockerPoints[i].Position);
        }
    }


    public override Array<Vector2> GetBlockerPoints()
    {
        return blockerOffsets;
    }
}
