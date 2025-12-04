using Godot;
using Godot.Collections;
using Snowdrama.Core;

[GlobalClass, Tool]
public partial class DecoratorPath2D : Path2D
{
    [Export] Array<PackedScene> decorations = new Array<PackedScene>();
    [Export] Vector2 distanceRange = new Vector2(250.0f, 500.0f);

    [ExportToolButton("Decorate Path!")] Callable OnDecorate => Callable.From(Decorate);



    [ExportGroup("Rotation Stuff")]
    [Export] bool alignToPath;
    [Export] Vector2 angleRange = new Vector2(0, 360);
    [ExportGroup("Scale Stuff")]
    [Export] Vector2 scaleRange = new Vector2(0.5f, 1.5f);

    [ExportGroup("Debug")]
    [Export] Array<Node2D> spawnedDecoations = new Array<Node2D>();

    bool internalWarn = false;
    int lastCount = 0;
    public void Decorate()
    {
        foreach (var node in spawnedDecoations)
        {
            node.QueueFree();
        }

        spawnedDecoations.Clear();

        int count = Mathf.FloorToInt(this.Curve.GetBakedLength() / distanceRange.X);

        if (count > 1500 && (count != lastCount || !internalWarn))
        {
            lastCount = count;
            internalWarn = true;
            Debug.LogError($"Trying to decorate and generating {count} objects, are you sure you want to do that? If this is intentional, press decorate again!");
            return;
        }

        for (float i = 0; i < this.Curve.GetBakedLength(); i += distanceRange.RandomBetweenXY())
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
            lastCount = 0;
            internalWarn = false;
        }
    }
}
