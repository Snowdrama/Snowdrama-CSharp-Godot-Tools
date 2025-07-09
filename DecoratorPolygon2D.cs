using Godot;
using Godot.Collections;
using Snowdrama.Core;
using System.Linq;

[Tool, GlobalClass]
public partial class DecoratorPolygon2D : Polygon2D
{
    [Export] Array<PackedScene> decorations = new Array<PackedScene>();
    [Export] Vector2 decorationDistance = new Vector2(1, 1);
    [Export] Vector2 decorationOffsetRange = new Vector2(1, 1);
    [Export] Vector2 angleRange = new Vector2(0, 360);
    [Export] Vector2 scaleRange = new Vector2(0.5f, 1.5f);

    [ExportToolButton("Decorate Area!!")] Callable OnDecorate => Callable.From(Decorate);

    [Export] Array<Node2D> spawnedDecoations = new Array<Node2D>();
    public void Decorate()
    {
        spawnedDecoations = new Array<Node2D>(spawnedDecoations.Where(IsInstanceValid).ToArray());
        foreach (var node in spawnedDecoations)
        {
            node.QueueFree();
        }

        spawnedDecoations.Clear();


        Vector2 min = Vector2.Inf;
        Vector2 max = -Vector2.Inf;

        foreach (Vector2 point in this.Polygon)
        {
            min = Vector2Extensions.Min(min, point);
            max = Vector2Extensions.Max(max, point);
        }
        var bounds = new Rect2(min, max - min);

        Debug.LogWarn($"Bounds! {bounds}");

        //make sure it's not 0 or it will be an infinite loop
        decorationDistance = decorationDistance.Clamp(0.1f, Mathf.Inf);


        int targetCount = Mathf.FloorToInt((bounds.Size.X / decorationDistance.X) * (bounds.Size.Y / decorationDistance.Y));
        if (targetCount > 1_000)
        {
            Debug.LogError($"Trying to spawn {targetCount} do you actually want to do that?");
            return;
        }
        else
        {
            Debug.LogWarn($"Starting to spawn {targetCount}");
        }
        for (float y = 0; y < bounds.Size.Y; y += decorationDistance.Y)
        {
            for (float x = 0; x < bounds.Size.X; x += decorationDistance.X)
            {
                var offset = new Vector2(
                    RandomAndNoise.RandomRange(decorationOffsetRange.X, decorationOffsetRange.Y),
                    RandomAndNoise.RandomRange(decorationOffsetRange.X, decorationOffsetRange.Y)
                );
                var pos = bounds.Position + new Vector2(x, y) + offset;

                if (Geometry2D.IsPointInPolygon(pos, this.Polygon))
                {
                    var newNode = (Node2D)decorations.GetRandom().Instantiate();
                    newNode.GlobalPosition = pos;
                    newNode.GlobalRotation = RandomAndNoise.RandomRange(angleRange.X, angleRange.Y);
                    newNode.Scale = Vector2.One * RandomAndNoise.RandomRange(scaleRange.X, scaleRange.Y);
                    this.AddChild(newNode, true);
                    spawnedDecoations.Add(newNode);
                    newNode.Owner = this.Owner;
                }
            }
        }

    }
}
