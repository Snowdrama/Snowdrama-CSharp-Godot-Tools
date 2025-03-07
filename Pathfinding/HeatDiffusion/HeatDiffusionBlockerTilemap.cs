using Godot;
using Godot.Collections;
using System;

public partial class HeatDiffusionBlockerTilemap : HeatDiffusionBlocker
{
    [Export] HeatDiffusionMap heatMap;
    [Export] TileMapLayer tileMap;
    //[Export] Vector2I gridSize = new Vector2I(100,100);
    Array<Vector2> blockerPoints = new Array<Vector2>();
    public override void _Ready()
    {
        base._Ready();
        for (int y = 0; y < heatMap.mapSize.Y; y++)
        {
            for (int x = 0; x < heatMap.mapSize.X; x++)
            {
                var tile = tileMap.GetCellTileData(new Vector2I(x, y));
                if (tile != null)
                {
                    blockerPoints.Add(new Vector2(x, y) * heatMap.CellSize);
                    Debug.Log($"[{new Vector2I(x, y)}]: {tile}");
                }
            }
        }
    }
    public override Array<Vector2> GetBlockerPoints()
    {
        return blockerPoints;
    }
}
