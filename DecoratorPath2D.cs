using Godot;
using Godot.Collections;
using Snowdrama.Core;

[GlobalClass, Tool]
public partial class DecoratorPath2D : Path2D
{
    [Export] Array<PackedScene> decorations = new Array<PackedScene>();
    [Export] float distance = 1.0f;

    [ExportToolButton("Decorate Path!")] Callable OnDecorate => Callable.From(Decorate);



    [ExportGroup("Rotation Stuff")]
    [Export] bool alignToPath;
    [Export] Vector2 angleRange = new Vector2(0, 360);
    [ExportGroup("Scale Stuff")]
    [Export] Vector2 scaleRange = new Vector2(0.5f, 1.5f);

    [ExportGroup("Debug")]
    [Export] Array<Node2D> spawnedDecoations = new Array<Node2D>();
    public void Decorate()
    {
        foreach (var node in spawnedDecoations)
        {
            node.QueueFree();
        }

        spawnedDecoations.Clear();

        for (float i = 0; i < this.Curve.GetBakedLength(); i += distance)
        {
            var pos = this.Curve.SampleBaked(i);
            var rot = RandomAndNoise.RandomRange(angleRange.X, angleRange.Y);
            if (alignToPath)
            {
                var transform = this.Curve.SampleBakedWithRotation(i);
                pos = transform.Origin;
                rot = transform.Rotation;
            }

            var newNode = (Node2D)decorations.GetRandom().Instantiate();

            newNode.GlobalPosition = pos;
            newNode.GlobalRotation = rot;

            newNode.Scale = Vector2.One * RandomAndNoise.RandomRange(scaleRange.X, scaleRange.Y);
            this.AddChild(newNode, true);
            spawnedDecoations.Add(newNode);
            newNode.Owner = this.Owner;
        }
    }
}
